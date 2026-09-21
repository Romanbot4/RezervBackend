using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Database;
using Application.Abstractions.DateTime;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Configurations;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class WaitlistExpirerTests
{
    private static readonly DateTime Now = new(2026, 6, 10, 9, 0, 0, DateTimeKind.Utc);

    private readonly Mock<IWaitlistRepository> _waitlistsMock;
    private readonly Mock<IBookingService> _bookingServiceMock;
    private readonly Mock<IDistributedLockService> _locksMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly WaitlistExpirer _sut; // System Under Test

    public WaitlistExpirerTests()
    {
        _waitlistsMock = new Mock<IWaitlistRepository>();
        _bookingServiceMock = new Mock<IBookingService>();
        _locksMock = new Mock<IDistributedLockService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var dateTimeMock = new Mock<IDateTime>();
        dateTimeMock.Setup(d => d.UtcNow).Returns(Now);

        var lockHandleMock = new Mock<ILockHandle>();
        _locksMock
            .Setup(l =>
                l.AcquireAsync(
                    It.IsAny<string>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(lockHandleMock.Object);

        _unitOfWorkMock
            .Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<ITransaction>().Object);

        _sut = new WaitlistExpirer(
            _waitlistsMock.Object,
            _bookingServiceMock.Object,
            _locksMock.Object,
            _unitOfWorkMock.Object,
            dateTimeMock.Object,
            Options.Create(new HangfireConfigurations()),
            new Mock<ILogger<WaitlistExpirer>>().Object
        );
    }

    private static WaitlistEntryEntity Entry(Guid scheduleId) =>
        new(
            id: Guid.NewGuid(),
            timetableScheduleId: scheduleId,
            customerId: Guid.NewGuid(),
            customerPackageId: Guid.NewGuid(),
            joinedAt: Now.AddHours(-2)
        );

    private void Schedules(params Guid[] ids) =>
        _waitlistsMock
            .Setup(w =>
                w.GetSchedulesWithEndedWaitingAsync(
                    Now,
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(ids);

    private void Waiting(Guid scheduleId, params WaitlistEntryEntity[] entries) =>
        _waitlistsMock
            .Setup(w => w.GetWaitingAsync(scheduleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entries);

    [Fact]
    public async Task Nothing_ended_releases_nothing()
    {
        Schedules();

        var released = await _sut.ReleaseEndedAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, released);
        _bookingServiceMock.Verify(
            b =>
                b.DropWaitlistAsync(
                    It.IsAny<WaitlistEntryEntity>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }

    [Fact]
    public async Task Every_entry_still_waiting_on_an_ended_class_is_dropped()
    {
        var scheduleId = Guid.NewGuid();
        Schedules(scheduleId);
        Waiting(scheduleId, Entry(scheduleId), Entry(scheduleId));

        var released = await _sut.ReleaseEndedAsync(TestContext.Current.CancellationToken);

        Assert.Equal(2, released);
        _bookingServiceMock.Verify(
            b =>
                b.DropWaitlistAsync(
                    It.IsAny<WaitlistEntryEntity>(),
                    It.IsAny<string>(),
                    Now,
                    It.IsAny<CancellationToken>()
                ),
            Times.Exactly(2)
        );
    }

    [Fact]
    public async Task A_busy_schedule_is_skipped_and_left_for_the_next_run()
    {
        var scheduleId = Guid.NewGuid();
        Schedules(scheduleId);
        Waiting(scheduleId, Entry(scheduleId));

        _locksMock
            .Setup(l =>
                l.AcquireAsync(
                    It.IsAny<string>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<TimeSpan>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync((ILockHandle?)null);

        var released = await _sut.ReleaseEndedAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, released);
        _bookingServiceMock.Verify(
            b =>
                b.DropWaitlistAsync(
                    It.IsAny<WaitlistEntryEntity>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }

    [Fact]
    public async Task One_bad_schedule_does_not_stop_the_others()
    {
        var bad = Guid.NewGuid();
        var good = Guid.NewGuid();
        Schedules(bad, good);
        Waiting(good, Entry(good));

        _waitlistsMock
            .Setup(w => w.GetWaitingAsync(bad, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.ReleaseEndedAsync(TestContext.Current.CancellationToken)
        );

        // the healthy schedule was still swept before the run reported itself as failed
        _bookingServiceMock.Verify(
            b =>
                b.DropWaitlistAsync(
                    It.IsAny<WaitlistEntryEntity>(),
                    It.IsAny<string>(),
                    Now,
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
