using Application.Abstractions.Messaging;
using Contract.TimetableSchedule;

namespace Application.Features.TimetableSchedule.UseCases.GetTimetableSchedules;

public record GetTimetableSchedulesQuery(Guid? BusinessId, DateOnly? Date)
    : IQuery<ICollection<TimetableScheduleResponse>>;
