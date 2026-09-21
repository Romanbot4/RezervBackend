using Core.Primitives.Entity;
using Domain.Errors;

namespace Domain.Entities;

/// A timetable schedule includes:
/// ● business
/// ● class name
/// ● instructor
/// ● start time
/// ● end time
/// ● available slots
///
/// Example
/// Yoga Class
/// 10:00 AM – 11:00 AM
/// ● Instructor: John Doe
/// ● 12 / 15 attendees
public class TimetableScheduleEntity(
    Guid id,
    Guid businessId,
    string className,
    string instructorName,
    DateTime startTime,
    DateTime endTime,
    int availableSlots,
    int bookedCount = 0
) : AggregateRoot(id), IHasTimestamps
{
    public Guid BusinessId { get; set; } = businessId;
    public string ClassName { get; set; } = className;
    public string InstructorName { get; set; } = instructorName;
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;
    public int AvailableSlots { get; set; } = availableSlots;
    public int BookedCount { get; set; } = bookedCount;
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //Join Entities
    public BusinessEntity Business { get; set; } = null!;
    public ICollection<BookingEntity> Bookings { get; set; } = [];
    public ICollection<WaitlistEntryEntity> WaitlistEntries { get; set; } = [];

    //Domain Logics
    public bool HasAvailableSlot()
    {
        return (AvailableSlots - BookedCount) > 0;
    }

    //Domain logics
    public bool HasStarted(DateTime now)
    {
        return StartTime <= now;
    }

    public static readonly TimeSpan RefundWindow = TimeSpan.FromHours(4);

    public bool QualifiesForRefund(DateTime now)
    {
        return now < StartTime - RefundWindow;
    }

    public void ReserveSlot()
    {
        if (!HasAvailableSlot())
        {
            throw BookingErrors.ScheduleFull();
        }

        BookedCount += 1;
    }

    public void ReleaseSlot()
    {
        if (BookedCount > 0)
        {
            BookedCount -= 1;
        }
    }
}
