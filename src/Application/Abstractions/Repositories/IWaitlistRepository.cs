using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IWaitlistRepository : IGenericRepository<WaitlistEntryEntity>
{
    public Task<bool> IsWaitingAsync(
        Guid customerId,
        Guid timetableScheduleId,
        CancellationToken cancellationToken = default
    );
}
