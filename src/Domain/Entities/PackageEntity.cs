using Core.Primitives.Entity;

namespace Domain.Entities;

/// <summary>
/// A package includes:
/// ● business
/// ● total credits
/// ● remaining credits
/// ● expiry date
///
/// Rules
/// 1. Expired packages cannot be used.
/// 2. Booking deducts 1 credit immediately.
/// 3. Remaining credits must be tracked.
/// 4. Package can only be used for timetable schedules under the same business.
/// 5. Customer must have at least 1 available credit to book.
/// </summary>
public class PackageEntity(
    Guid id,
    Guid businessId,
    string name,
    int credits,
    int validityDays,
    decimal price,
    bool isActive = true
) : AggregateRoot(id), IHasTimestamps
{
    public Guid BusinessId { get; set; } = businessId;
    public string Name { get; set; } = name;
    public int Credits { get; set; } = credits;
    public int ValidityDays { get; set; } = validityDays;
    public decimal Price { get; set; } = price;
    public bool IsActive { get; set; } = isActive;
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //Join Entities
    public BusinessEntity Business { get; set; } = null!;

    //Domain Logics
    public CustomerPackageEntity PurchaseFor(Guid customerId, DateTime now)
    {
        return new CustomerPackageEntity(
            id: Guid.NewGuid(),
            customerId: customerId,
            packageId: Id,
            businessId: BusinessId,
            totalCredits: Credits,
            remainingCredits: Credits,
            reservedCredits: 0,
            purchasedAt: now,
            expiresAt: now.AddDays(ValidityDays)
        );
    }
}
