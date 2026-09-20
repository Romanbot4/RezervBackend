using Core.Primitives.Entity;

namespace Domain.Entities;

/// <summary>
/// I dont see any business properties in pdf file
/// Defined a simple business entity with just
/// name and active boolean flag
/// </summary>
public class BusinessEntity(Guid id, string name, bool isActive = true)
    : AggregateRoot(id),
        IHasTimestamps
{
    public string Name { get; set; } = name;
    public bool IsActive { get; set; } = isActive;
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
