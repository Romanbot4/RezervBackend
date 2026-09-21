using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class WaitlistRepository(IDbContext dbContext)
    : GenericRepository<WaitlistEntryEntity>(dbContext),
        IWaitlistRepository
{
    public Task<WaitlistEntryEntity?> GetNextWaitingAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        return _query
            .Include(w => w.CustomerPackage)
            .Where(w => w.TimetableScheduleId == id && w.Status == WaitlistStatus.Waiting)
            .OrderBy(w => w.JoinedAt)
            .ThenBy(w => w.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    // The result can be stale by the time we act on it, so the authoritative
    // read happens in GetWaitingAsync once the schedule lock is held.
    public async Task<IReadOnlyList<Guid>> GetSchedulesWithEndedWaitingAsync(
        DateTime now,
        int limit,
        CancellationToken cancellationToken = default
    )
    {
        return await _query
            .AsNoTracking()
            .Where(w => w.Status == WaitlistStatus.Waiting && w.TimetableSchedule.EndTime <= now)
            .Select(w => w.TimetableScheduleId)
            .Distinct()
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    // Tracked on purpose. DropWaitlistAsync calls entry.Expire() and relies on the change tracker.
    public async Task<IReadOnlyList<WaitlistEntryEntity>> GetWaitingAsync(
        Guid timetableScheduleId,
        CancellationToken cancellationToken = default
    )
    {
        return await _query
            .Where(w =>
                w.TimetableScheduleId == timetableScheduleId && w.Status == WaitlistStatus.Waiting
            )
            .OrderBy(w => w.JoinedAt)
            .ThenBy(w => w.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsWaitingAsync(
        Guid customerId,
        Guid timetableScheduleId,
        CancellationToken cancellationToken = default
    )
    {
        return _query
            .AsNoTracking()
            .AnyAsync(
                w =>
                    w.CustomerId == customerId
                    && w.TimetableScheduleId == timetableScheduleId
                    && w.Status == WaitlistStatus.Waiting,
                cancellationToken
            );
    }
}
