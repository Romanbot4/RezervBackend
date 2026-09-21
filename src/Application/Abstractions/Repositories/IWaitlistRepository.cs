using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

using System;

public interface IWaitlistRepository : IGenericRepository<WaitlistEntryEntity>
{
    Task<WaitlistEntryEntity?> GetNextWaitingAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Guid>> GetSchedulesWithEndedWaitingAsync(
        DateTime now,
        int limit,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<WaitlistEntryEntity>> GetWaitingAsync(
        Guid timetableScheduleId,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsWaitingAsync(
        Guid customerId,
        Guid timetableScheduleId,
        CancellationToken cancellationToken = default
    );
}
