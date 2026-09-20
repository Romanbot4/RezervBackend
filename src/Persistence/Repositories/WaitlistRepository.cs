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
