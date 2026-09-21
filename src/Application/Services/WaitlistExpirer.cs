using Application.Abstractions.Database;
using Application.Abstractions.DateTime;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class WaitlistExpirer(
    IWaitlistRepository waitlists,
    IBookingService bookingService,
    IDistributedLockService locks,
    IUnitOfWork unitOfWork,
    IDateTime dateTime,
    IOptions<HangfireConfigurations> configurations,
    ILogger<WaitlistExpirer> logger
) : IWaitlistExpirer
{
    private const string Reason = "Class ended while still on the waitlist";

    public async Task<int> ReleaseEndedAsync(CancellationToken cancellationToken = default)
    {
        var now = dateTime.UtcNow;

        var scheduleIds = await waitlists.GetSchedulesWithEndedWaitingAsync(
            now,
            configurations.Value.BatchSize,
            cancellationToken
        );

        var released = 0;
        var failed = 0;

        foreach (var scheduleId in scheduleIds)
        {
            try
            {
                released += await ReleaseScheduleAsync(scheduleId, now, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                failed++;

                logger.LogError(
                    exception,
                    "Could not release the waitlist reservations on schedule {ScheduleId}",
                    scheduleId
                );
            }
        }

        if (released > 0 || failed > 0)
        {
            logger.LogInformation(
                "Released {Released} waitlist reservations across {Schedules} ended schedules, {Failed} failed",
                released,
                scheduleIds.Count,
                failed
            );
        }

        if (failed > 0)
        {
            throw new InvalidOperationException(
                $"{failed} of {scheduleIds.Count} schedules could not be swept"
            );
        }

        return released;
    }

    private async Task<int> ReleaseScheduleAsync(
        Guid scheduleId,
        DateTime now,
        CancellationToken cancellationToken
    )
    {
        await using var scheduleLock = await locks.AcquireAsync(
            LockKeys.Schedule(scheduleId),
            LockKeys.Ttl,
            LockKeys.Wait,
            cancellationToken
        );

        if (scheduleLock is null)
        {
            // A busy schedule is normal for a sweep, not an error. It gets picked up next tick.
            logger.LogInformation(
                "Skipped schedule {ScheduleId}, it was busy. Picking it up on the next run",
                scheduleId
            );

            return 0;
        }

        var entries = await waitlists.GetWaitingAsync(scheduleId, cancellationToken);

        if (entries.Count == 0)
        {
            return 0;
        }

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        foreach (var entry in entries)
        {
            await bookingService.DropWaitlistAsync(entry, Reason, now, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation(
            "Released {Count} credit reservations on schedule {ScheduleId} because the class already ended",
            entries.Count,
            scheduleId
        );

        return entries.Count;
    }
}
