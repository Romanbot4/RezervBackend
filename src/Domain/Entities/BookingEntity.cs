using Core.Primitives.Entity;

namespace Domain.Entities;

/// <summary>
/// Booking
/// Users can book timetable schedules.
/// Rules
/// 1. Package business must match timetable schedule business.
/// 2. Customer must have sufficient credits.
/// 3. Users cannot book overlapping timetable schedules.
/// 4. Booking deducts 1 package credit immediately.
/// 5. Booking count cannot exceed available slots.
/// Cancellation
/// Users may cancel bookings.
/// Rules
/// 1. If cancelled more than 4 hours before class start time:
/// ○ 1 credit should be refunded
/// 2. If cancelled within 4 hours:
/// ○ no refund
/// </summary>
public class BookingEntity(
    Guid id,
    Guid customerId,
    Guid timetableScheduleId,
    Guid customerPackageId,
    DateTime bookedAt
) : AggregateRoot(id), IHasTimestamps
{
    public Guid CustomerId { get; set; } = customerId;
    public Guid TimetableScheduleId { get; set; } = timetableScheduleId;
    public Guid CustomerPackageId { get; set; } = customerPackageId;
    public DateTime BookedAt { get; set; } = bookedAt;
    public DateTime? CancelledAt { get; set; }
    public bool RefundApplied { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Join Entities
    public CustomerEntity Customer { get; set; } = null!;
    public TimetableScheduleEntity TimetableSchedule { get; set; } = null!;
    public CustomerPackageEntity CustomerPackage { get; set; } = null!;
}
