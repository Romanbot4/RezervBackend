using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ITimetableScheduleRepository : IGenericRepository<TimetableScheduleEntity>
{
    Task<bool> TryReserveSlotAsync(Guid scheduleId, CancellationToken cancellationToken);

    Task<bool> ReleaseSlotAsync(Guid scheduleId, CancellationToken cancellationToken);
}
