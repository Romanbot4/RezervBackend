using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ICustomerPackageRepository : IGenericRepository<CustomerPackageEntity>
{
    Task<bool> TryConsumeCreditAsync(Guid customerPackageId, CancellationToken cancellationToken);

    Task<bool> TryReserveCreditAsync(Guid customerPackageId, CancellationToken cancellationToken);

    Task<bool> TryConsumeReservedCreditAsync(
        Guid customerPackageId,
        CancellationToken cancellationToken
    );

    Task<bool> RefundCreditAsync(Guid customerPackageId, CancellationToken cancellationToken);

    Task<bool> ReleaseReservationAsync(Guid customerPackageId, CancellationToken cancellationToken);

    Task<CustomerPackageEntity?> ReloadAsync(
        Guid customerPackageId,
        CancellationToken cancellationToken
    );
}
