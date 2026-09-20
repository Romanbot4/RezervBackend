using Core.Primitives.Entity;

namespace Domain.Entities;

public class CustomerPackageEntity(
    Guid id,
    Guid customerId,
    Guid packageId,
    Guid businessId,
    int totalCredits,
    int remainingCredits,
    int reservedCredits,
    DateTime purchasedAt,
    DateTime expiresAt
) : AggregateRoot(id), IHasTimestamps
{
    public Guid CustomerId { get; set; } = customerId;
    public Guid PackageId { get; set; } = packageId;
    public Guid BusinessId { get; set; } = businessId;
    public int TotalCredits { get; set; } = totalCredits;
    public int RemainingCredits { get; set; } = remainingCredits;
    public int ReservedCredits { get; set; } = reservedCredits;
    public DateTime PurchasedAt { get; set; } = purchasedAt;
    public DateTime ExpiresAt { get; set; } = expiresAt;
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Join Entities
    public CustomerEntity Customer { get; set; } = null!;
    public PackageEntity Package { get; set; } = null!;
    public BusinessEntity Business { get; set; } = null!;
}
