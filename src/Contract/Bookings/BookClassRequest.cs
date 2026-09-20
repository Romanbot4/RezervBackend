namespace Contract.Bookings;

/// <param name="JoinWaitlistIfFull">
/// When the class is full, join the waitlist. Defaults true. If not will fail.
/// </param>
public record BookClassRequest(
    Guid ScheduleId,
    Guid CustomerPackageId,
    bool JoinWaitlistIfFull = true
);
