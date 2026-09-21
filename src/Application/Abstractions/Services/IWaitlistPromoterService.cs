using Domain.Entities;

namespace Application.Abstractions.Services;

public interface IWaitlistPromoter
{
    /// <summary>
    /// Books the first eligible customer waiting on into the slot that
    /// has just been freed, return null when nobody could be promoted.
    /// </summary>
    Task<BookingEntity?> PromoteNextAsync(
        TimetableScheduleEntity schedule,
        CancellationToken cancellationToken = default
    );
}
