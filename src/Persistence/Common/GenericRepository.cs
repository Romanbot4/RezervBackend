using Application.Abstractions.Database;
using Application.Database;
using Core.Exception.NetworkException;
using Core.Primitives.Entity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common;

public abstract class GenericRepository<TEntity>(IDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : AggregateRoot
{
    protected readonly IDbContext _dbContext = dbContext;
    protected readonly IQueryable<TEntity> _query = dbContext.Set<TEntity>().AsQueryable();

    public virtual async Task<TEntity?> GetByIdAsync(
        Guid id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? alterQuery = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = _query;
        if (alterQuery != null)
        {
            query = alterQuery(query) ?? query;
        }

        return await query.Where(e => e.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<ICollection<TEntity>> GetRangeAsync(
        int? limit,
        int? offset,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? alterQuery = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = _query;
        if (alterQuery != null)
        {
            query = alterQuery(query) ?? query;
        }
        if (offset != null)
        {
            query = query.Skip((int)offset);
        }
        if (limit != null)
        {
            query = query.Take((int)limit);
        }
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> InsertAsync(
        TEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.InsertAsync(entity, cancellationToken);
    }

    public virtual async Task InsertRangeAsync(
        IReadOnlyCollection<TEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.InsertRangeAsync(entities, cancellationToken);
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Remove(entity);
    }

    public void RemoveById(Guid id)
    {
        var effectedRows = _dbContext.Set<TEntity>().Where(e => e.Id == id).ExecuteDelete();
        if (effectedRows == 0)
        {
            throw new NotFoundException($"Entity with id {id} not found");
        }
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbContext.RemoveRange(entities);
    }
}
