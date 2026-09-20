using Core.Primitives.Entity;

namespace Domain.Entities;

public class CreditTransactionEntity(
    Guid id,
    Guid customerPackageId,
    int amount,
    string reason,
    DateTime occurredAt
) : AggregateRoot(id), IHasTimestamps
{
    public Guid CustomerPackageId { get; set; } = customerPackageId;
    public int Amount { get; set; } = amount;
    public string Reason { get; set; } = reason;
    public DateTime OccurredAt { get; set; } = occurredAt;
    public Guid? BookingId { get; set; }
    public Guid? WaitlistEntryId { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //Join Entities
    public CustomerPackageEntity CustomerPackage { get; set; } = null!;
}
