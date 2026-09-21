using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class TimetableScheduleRepository(IDbContext dbContext)
    : GenericRepository<TimetableScheduleEntity>(dbContext),
        ITimetableScheduleRepository
{
    public async Task<bool> TryReserveSlotAsync(
        Guid scheduleId,
        CancellationToken cancellationToken
    )
    {
        var affected = await _query
            .Where(s => s.Id == scheduleId && s.BookedCount < s.AvailableSlots)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(s => s.BookedCount, s => s.BookedCount + 1),
                cancellationToken
            );

        return affected == 1;
    }

    public async Task<bool> ReleaseSlotAsync(Guid scheduleId, CancellationToken cancellationToken)
    {
        var affected = await _query
            .Where(s => s.Id == scheduleId && s.BookedCount >= 1)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(s => s.BookedCount, s => s.BookedCount - 1),
                cancellationToken
            );

        return affected == 1;
    }
}
