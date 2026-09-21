namespace Contract.TimetableSchedule;

public record TimetableScheduleResponse(
    Guid ScheduleId,
    string ClassName,
    string InstructorName,
    DateTime StartTime,
    DateTime EndTime,
    int AttendanceCount,
    int AvailableSlots,
    bool IsFull,
    Guid BusinessId,
    string BusinessName
);
