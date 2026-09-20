using Core.Primitives.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Database;

public interface IDbContext
{
    public DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity;

    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : Entity;

    public Task<TEntity> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken)
        where TEntity : Entity;

    public Task<TEntity> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : Entity;

    public Task InsertRangeAsync<TEntity>(
        IReadOnlyCollection<TEntity> entities,
        CancellationToken cancellationToken
    )
        where TEntity : Entity;

    public void Remove<TEntity>(TEntity entity)
        where TEntity : Entity;

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity;
}
