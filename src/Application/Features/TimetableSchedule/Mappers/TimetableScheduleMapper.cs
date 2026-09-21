using Contract.TimetableSchedule;
using Domain.Entities;

namespace Application.Features.TimetableSchedule.Mappers;

public static class TimetableScheduleMapper
{
    public static TimetableScheduleResponse ToTimetableScheduleResponse(
        this TimetableScheduleEntity schedule
    )
    {
        return new TimetableScheduleResponse(
            ScheduleId: schedule.Id,
            ClassName: schedule.ClassName,
            InstructorName: schedule.InstructorName,
            StartTime: schedule.StartTime,
            EndTime: schedule.EndTime,
            AttendanceCount: schedule.BookedCount,
            AvailableSlots: schedule.AvailableSlots,
            IsFull: !schedule.HasAvailableSlot(),
            BusinessId: schedule.BusinessId,
            BusinessName: schedule.Business.Name
        );
    }
}
