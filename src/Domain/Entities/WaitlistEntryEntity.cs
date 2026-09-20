using Core.Primitives.Entity;

namespace Domain.Entities;

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
public class WaitlistEntryEntity(
    Guid id,
    Guid timetableScheduleId,
    Guid customerId,
    Guid customerPackageId,
    DateTime joinedAt
) : AggregateRoot(id), IHasTimestamps
{
    public Guid TimetableScheduleId { get; set; } = timetableScheduleId;
    public Guid CustomerId { get; set; } = customerId;
    public Guid CustomerPackageId { get; set; } = customerPackageId;
    public DateTime JoinedAt { get; set; } = joinedAt;
    public DateTime? PromotedAt { get; set; }
    public Guid? BookingId { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //Join Entities
    public CustomerEntity Customer { get; set; } = null!;
    public TimetableScheduleEntity TimetableSchedule { get; set; } = null!;
    public CustomerPackageEntity CustomerPackage { get; set; } = null!;
}
