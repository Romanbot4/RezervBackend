using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class CustomerPackageRepository(IDbContext dbContext)
    : GenericRepository<CustomerPackageEntity>(dbContext),
        ICustomerPackageRepository
{
    public async Task RefundCreditAsync(Guid id, CancellationToken cancellationToken)
    {
        var customerPackage = await _query
            .Where(cp => cp.Id == id && cp.RemainingCredits < cp.TotalCredits) //extra check to ensure user spend credit already.
            .SingleAsync(cancellationToken);

        customerPackage.RemainingCredits++;
    }

    public async Task ReleaseReservationAsync(Guid id, CancellationToken cancellationToken)
    {
        var customerPackage = await _query
            .Where(cp => cp.Id == id && cp.ReservedCredits >= 1)
            .SingleAsync(cancellationToken);
        customerPackage.ReservedCredits--;
    }
}
