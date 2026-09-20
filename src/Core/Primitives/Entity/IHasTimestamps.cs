namespace Core.Primitives.Entity;

public interface IHasTimestamps
{
    DateTime AddedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
