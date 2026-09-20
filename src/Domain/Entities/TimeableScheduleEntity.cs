using Core.Primitives.Entity;

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
}
