using Application.Abstractions.Database;
using Application.Abstractions.DateTime;
using Application.Database;
using Core.Exception.NetworkException;
using Core.Primitives.Entity;
using Core.Primitives.Event;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Common;

namespace Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDateTime dateTime,
    IMediator mediator
) : DbContext(options), IUnitOfWork, IDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity
    {
        return base.Set<TEntity>();
    }

    EntityEntry<TEntity> IDbContext.Entry<TEntity>(TEntity entity)
    {
        return Entry(entity);
    }

    public async Task<ITransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    )
    {
        return new EfTransaction(await Database.BeginTransactionAsync(cancellationToken));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        var result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEvents(cancellationToken);
        return result;
    }

    private void UpdateTimestamps()
    {
        DateTime now = dateTime.UtcNow;

        foreach (EntityEntry<IHasTimestamps> entry in ChangeTracker.Entries<IHasTimestamps>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.AddedAt = now;
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken)
        where TEntity : Entity
    {
        var entity = await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity
            ?? throw new NotFoundException($"{typeof(TEntity).Name} with id {id} not found");
    }

    public async Task<TEntity> InsertAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken
    )
        where TEntity : Entity
    {
        try
        {
            EntityEntry<TEntity> entry = await Set<TEntity>().AddAsync(entity, cancellationToken);
            return entry.Entity;
        }
        catch (Exception)
        {
            throw new ValidationException("Cannot insert entity into database.", []);
        }
    }

    public async Task InsertRangeAsync<TEntity>(
        IReadOnlyCollection<TEntity> entities,
        CancellationToken cancellationToken
    )
        where TEntity : Entity
    {
        try
        {
            await Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }
        catch (Exception)
        {
            throw new ValidationException("Cannot bulk insert entities.", []);
        }
    }

    public void UpdateRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity
    {
        Set<TEntity>().UpdateRange(entities);
    }

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : Entity
    {
        Set<TEntity>().Remove(entity);
    }

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity
    {
        foreach (TEntity entity in entities)
        {
            Set<TEntity>().Remove(entity);
        }
    }

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        List<EntityEntry<AggregateRoot>> aggregateRoots =
        [
            .. ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents.Count != 0),
        ];

        List<IDomainEvent> domainEvents =
        [
            .. aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents),
        ];

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        IEnumerable<Task> tasks = domainEvents.Select(domainEvent =>
            mediator.Publish(domainEvent, cancellationToken)
        );

        await Task.WhenAll(tasks);
    }
}
