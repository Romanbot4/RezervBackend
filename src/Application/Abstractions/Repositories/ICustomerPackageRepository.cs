using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ICustomerPackageRepository : IGenericRepository<CustomerPackageEntity>
{
    Task RefundCreditAsync(Guid customerPackageId, CancellationToken cancellationToken);
    Task ReleaseReservationAsync(Guid customerPackageId, CancellationToken cancellationToken);
}
