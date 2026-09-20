namespace Core.Primitives.Entity;

public abstract class Entity(Guid id)
{
    public Guid Id { get; set; } = id;
}