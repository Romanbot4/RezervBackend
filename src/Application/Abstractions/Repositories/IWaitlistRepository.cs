using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IWaitlistRepository : IGenericRepository<WaitlistEntryEntity>
{
    Task<WaitlistEntryEntity?> GetNextWaitingAsync(Guid id, CancellationToken cancellationToken);

    public Task<bool> IsWaitingAsync(
        Guid customerId,
        Guid timetableScheduleId,
        CancellationToken cancellationToken = default
    );
}
