using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Features.TimetableSchedule.Mappers;
using Contract.TimetableSchedule;
using Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TimetableSchedule.UseCases.GetTimetableSchedules;

public class GetTimetableSchedulesQueryHandler(ITimetableScheduleRepository schedules)
    : IQueryHandler<GetTimetableSchedulesQuery, ICollection<TimetableScheduleResponse>>
{
    public async Task<Result<ICollection<TimetableScheduleResponse>>> Handle(
        GetTimetableSchedulesQuery request,
        CancellationToken cancellationToken
    )
    {
        var dayStart = request.Date?.ToDateTime(TimeOnly.MinValue);
        var dayEnd = dayStart?.AddDays(1);

        var matches = await schedules.GetRangeAsync(
            alterQuery: query =>
                query
                    .AsNoTracking()
                    .Include(schedule => schedule.Business)
                    .Where(schedule =>
                        request.BusinessId == null || schedule.BusinessId == request.BusinessId
                    )
                    .Where(schedule =>
                        dayStart == null
                        || (schedule.StartTime >= dayStart && schedule.StartTime < dayEnd)
                    )
                    .OrderBy(schedule => schedule.StartTime),
            cancellationToken: cancellationToken
        );

        ICollection<TimetableScheduleResponse> response =
        [
            .. matches.Select(schedule => schedule.ToTimetableScheduleResponse()),
        ];

        return Result<ICollection<TimetableScheduleResponse>>.Success(response);
    }
}
