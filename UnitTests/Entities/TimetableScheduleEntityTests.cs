using System;
using Core.Exception.NetworkException;
using Domain.Entities;
using Xunit;

namespace UnitTests.Entities;

public class TimetableScheduleEntityTests
{
    private static readonly DateTime Now = new(2026, 6, 10, 9, 0, 0, DateTimeKind.Utc);

    private static TimetableScheduleEntity Schedule(int slots = 15, int booked = 0) =>
        new(
            id: Guid.NewGuid(),
            businessId: Guid.NewGuid(),
            className: "Yoga Class",
            instructorName: "John Doe",
            startTime: Now.AddDays(1),
            endTime: Now.AddDays(1).AddHours(1),
            availableSlots: slots,
            bookedCount: booked
        );

    [Fact]
    public void ReserveSlot_raises_the_attendance_count()
    {
        var sut = Schedule(slots: 15, booked: 12);

        sut.ReserveSlot();

        Assert.Equal(13, sut.BookedCount);
        Assert.True(sut.HasAvailableSlot());
    }

    [Fact]
    public void Booking_the_last_slot_fills_the_class()
    {
        var sut = Schedule(slots: 5, booked: 4);

        sut.ReserveSlot();

        Assert.Equal(5, sut.BookedCount);
        Assert.False(sut.HasAvailableSlot());
    }

    [Fact]
    public void A_full_class_refuses_another_booking()
    {
        var sut = Schedule(slots: 5, booked: 5);

        Assert.Throws<UnprocessableException>(sut.ReserveSlot);
        Assert.Equal(5, sut.BookedCount);
    }

    [Fact]
    public void Attendance_can_never_be_pushed_past_capacity()
    {
        var sut = Schedule(slots: 5);

        for (var i = 0; i < 5; i++)
        {
            sut.ReserveSlot();
        }

        Assert.Throws<UnprocessableException>(sut.ReserveSlot);
        Assert.Equal(sut.AvailableSlots, sut.BookedCount);
    }

    [Fact]
    public void Cancelling_gives_the_seat_back()
    {
        var sut = Schedule(slots: 5, booked: 5);

        sut.ReleaseSlot();

        Assert.Equal(4, sut.BookedCount);
        Assert.True(sut.HasAvailableSlot());
    }

    [Fact]
    public void Releasing_an_empty_class_cannot_drive_attendance_negative()
    {
        var sut = Schedule(slots: 5, booked: 0);

        sut.ReleaseSlot();

        // A negative count would silently hand out more capacity than the class has.
        Assert.Equal(0, sut.BookedCount);
    }

    [Fact]
    public void A_cancellation_followed_by_a_promotion_leaves_attendance_unchanged()
    {
        var sut = Schedule(slots: 5, booked: 5);

        sut.ReleaseSlot();
        sut.ReserveSlot();

        Assert.Equal(5, sut.BookedCount);
    }

    [Fact]
    public void QualifiesForRefund_follows_the_current_start_time()
    {
        var sut = Schedule();

        Assert.True(sut.QualifiesForRefund(sut.StartTime.AddHours(-4).AddMinutes(-1)));
        Assert.False(sut.QualifiesForRefund(sut.StartTime.AddHours(-4)));
        Assert.False(sut.QualifiesForRefund(sut.StartTime.AddHours(-3)));

        // Reads the property, not the captured constructor argument: rescheduling the class moves
        // the refund window with it.
        sut.StartTime = sut.StartTime.AddDays(7);
        Assert.True(sut.QualifiesForRefund(Now.AddDays(1)));
    }
}
