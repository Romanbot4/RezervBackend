using Application.Abstractions.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence.Common;

public sealed class EfTransaction(IDbContextTransaction transaction) : ITransaction
{
    private bool completed;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await transaction.CommitAsync(cancellationToken);
        completed = true;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await transaction.RollbackAsync(cancellationToken);
        completed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (!completed)
        {
            await transaction.RollbackAsync();
        }

        await transaction.DisposeAsync();
    }
}
