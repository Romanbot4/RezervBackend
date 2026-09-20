using Application.Abstractions.Database.Types;
using Core.Primitives.Entity;

namespace Application.Abstractions.Database;

public interface IGenericRepository<TEntity>
    where TEntity : AggregateRoot
{
    Task<TEntity> GetByIdAsync(
        Guid id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? alterQuery = null,
        CancellationToken cancellationToken = default
    );

    Task<ICollection<TEntity>> GetRangeAsync(
        int? limit = null,
        int? offset = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? alterQuery = null,
        CancellationToken cancellationToken = default
    );

    Task<PaginatedResult<TEntity>> GetPaginatedAsync(
        int pageSize,
        int pageIndex,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? alterQuery = null,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task InsertRangeAsync(
        IReadOnlyCollection<TEntity> entities,
        CancellationToken cancellationToken = default
    );

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void RemoveById(Guid id);
}
