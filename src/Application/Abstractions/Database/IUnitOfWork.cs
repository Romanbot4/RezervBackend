namespace Application.Abstractions.Database;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
