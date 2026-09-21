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
    public async Task<bool> TryConsumeCreditAsync(Guid id, CancellationToken cancellationToken)
    {
        var affected = await _query
            .Where(cp => cp.Id == id && cp.RemainingCredits - cp.ReservedCredits >= 1)
            .ExecuteUpdateAsync(
                setters =>
                    setters.SetProperty(cp => cp.RemainingCredits, cp => cp.RemainingCredits - 1),
                cancellationToken
            );

        return affected == 1;
    }

    public async Task<bool> TryReserveCreditAsync(Guid id, CancellationToken cancellationToken)
    {
        var affected = await _query
            .Where(cp => cp.Id == id && cp.RemainingCredits - cp.ReservedCredits >= 1)
            .ExecuteUpdateAsync(
                setters =>
                    setters.SetProperty(cp => cp.ReservedCredits, cp => cp.ReservedCredits + 1),
                cancellationToken
            );

        return affected == 1;
    }

    public async Task<bool> TryConsumeReservedCreditAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var affected = await _query
            .Where(cp => cp.Id == id && cp.ReservedCredits >= 1 && cp.RemainingCredits >= 1)
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(cp => cp.RemainingCredits, cp => cp.RemainingCredits - 1)
                        .SetProperty(cp => cp.ReservedCredits, cp => cp.ReservedCredits - 1),
                cancellationToken
            );

        return affected == 1;
    }

    public async Task<bool> RefundCreditAsync(Guid id, CancellationToken cancellationToken)
    {
        var affected = await _query
            .Where(cp => cp.Id == id && cp.RemainingCredits < cp.TotalCredits)
            .ExecuteUpdateAsync(
                setters =>
                    setters.SetProperty(cp => cp.RemainingCredits, cp => cp.RemainingCredits + 1),
                cancellationToken
            );

        return affected == 1;
    }

    public async Task<bool> ReleaseReservationAsync(Guid id, CancellationToken cancellationToken)
    {
        var affected = await _query
            .Where(cp => cp.Id == id && cp.ReservedCredits >= 1)
            .ExecuteUpdateAsync(
                setters =>
                    setters.SetProperty(cp => cp.ReservedCredits, cp => cp.ReservedCredits - 1),
                cancellationToken
            );

        return affected == 1;
    }

    public Task<CustomerPackageEntity?> ReloadAsync(Guid id, CancellationToken cancellationToken)
    {
        return _query.AsNoTracking().FirstOrDefaultAsync(cp => cp.Id == id, cancellationToken);
    }
}
