using Application.Abstractions.Database;
using Application.Abstractions.DateTime;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Core.Exception.NetworkException;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services;

/// <summary>
/// Waitlist
/// If a timetable schedule becomes full:
/// Users may join a waitlist.
///
/// Rules
/// 1. Waitlist should follow FIFO.
/// 2. If a booked user cancels:
///     ○ first waitlist user becomes booked automatically
/// 3. 1 credit should be deducted when promoted to booked.
/// 4. If class ends and user remains in waitlist:
///     ○ credit reservation should be released / no deduction applied.
/// </summary>
public class WaitlistPromoter(
    IWaitlistRepository waitlists,
    IBookingService bookingService,
    ICurrentUserService currentUser,
    IDateTime dateTime,
    IUnitOfWork unitOfWork,
    ILogger<WaitlistPromoter> logger
) : IWaitlistPromoter
{
    public async Task<BookingEntity?> PromoteNextAsync(
        TimetableScheduleEntity schedule,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = currentUser.CustomerId ?? throw new AuthRequiredException();

        var now = dateTime.UtcNow;

        while (true)
        {
            var entry = await waitlists.GetNextWaitingAsync(schedule.Id, cancellationToken);

            if (entry is null)
            {
                return null;
            }

            var isStillEligible = IsStillEligible(entry, schedule, now);

            if (!isStillEligible)
            {
                await DropAsync(
                    entry,
                    "Package expired or no longer valid for this business",
                    now,
                    cancellationToken
                );
                continue;
            }

            var customerPackage = entry.CustomerPackage;

            // Booking already have business rules check so I'll reuse.
            // Will throw error if something went wrong.
            var booking = await bookingService.PromoteFromWaitlistAsync(
                new BookingContext(
                    CustomerId: customerId,
                    Schedule: schedule,
                    CustomerPackage: customerPackage,
                    Now: now
                ),
                cancellationToken
            );

            // Bbove step will throws error if not success. So, no success check.
            entry.Promote(now, booking.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Promoted customer {CustomerId} from the waitlist into schedule {ScheduleId} as booking {BookingId}",
                entry.CustomerId,
                schedule.Id,
                booking.Id
            );

            return booking;
        }
    }

    private static bool IsStillEligible(
        WaitlistEntryEntity entry,
        TimetableScheduleEntity schedule,
        DateTime now
    )
    {
        var customerPackage = entry.CustomerPackage;

        return !customerPackage.IsExpired(now)
            && customerPackage.BelongsToBusiness(schedule.BusinessId);
    }

    private async Task DropAsync(
        WaitlistEntryEntity entry,
        string reason,
        DateTime now,
        CancellationToken cancellationToken
    )
    {
        await bookingService.DropWaitlistAsync(entry, reason, now, cancellationToken);

        logger.LogInformation(
            "Dropped waitlist entry {WaitlistEntryId} and released its credit reservation: {Reason}",
            entry.Id,
            reason
        );
    }
}
