using Core.Primitives.Event;

namespace Core.Primitives.Entity;

public abstract class AggregateRoot(Guid id) : Entity(id)
{
    private readonly ICollection<IDomainEvent> domainEvents = [];

    public ICollection<IDomainEvent> DomainEvents => domainEvents;

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}
