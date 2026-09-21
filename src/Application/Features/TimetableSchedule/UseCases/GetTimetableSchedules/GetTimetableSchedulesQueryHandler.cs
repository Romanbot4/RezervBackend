using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Features.TimetableSchedule.Mappers;
using Contract.TimetableSchedule;
using Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TimetableSchedule.UseCases.GetTimetableSchedules;

public class GetTimetableSchedulesQueryHandler(
    ITimetableScheduleRepository schedules,
    ICacheService cache
) : IQueryHandler<GetTimetableSchedulesQuery, ICollection<TimetableScheduleResponse>>
{
    public async Task<Result<ICollection<TimetableScheduleResponse>>> Handle(
        GetTimetableSchedulesQuery request,
        CancellationToken cancellationToken
    )
    {
        var dayStart = request.Date?.ToDateTime(TimeOnly.MinValue);
        var dayEnd = dayStart?.AddDays(1);

        var response = await cache.GetOrSetAsync(
            CacheKeys.Timetable(request.BusinessId, request.Date),
            async token => await LoadAsync(dayStart, dayEnd, request, token),
            CacheKeys.TimetableTtl,
            cancellationToken
        );

        return Result<ICollection<TimetableScheduleResponse>>.Success(response);
    }

    private async Task<List<TimetableScheduleResponse>> LoadAsync(
        DateTime? dayStart,
        DateTime? dayEnd,
        GetTimetableSchedulesQuery request,
        CancellationToken cancellationToken
    )
    {
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

        return [.. matches.Select(schedule => schedule.ToTimetableScheduleResponse())];
    }
}
