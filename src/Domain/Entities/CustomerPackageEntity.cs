using Core.Primitives.Entity;
using Domain.Errors;

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

    //Domain Logics
    /// <summary>
    /// Credits that are neither spent nor already promised to a waitlist entry
    /// </summary>
    public int AvailableCredits => RemainingCredits - ReservedCredits;

    public bool BelongsToBusiness(Guid businessId)
    {
        return this.BusinessId == businessId;
    }

    public bool BelongsToCustomer(Guid customerId)
    {
        return this.CustomerId == customerId;
    }

    public void ConsumeCredits(int count, DateTime now)
    {
        if (IsExpired(now))
        {
            throw BookingErrors.PackageExpired(now);
        }

        if (AvailableCredits < count)
        {
            throw BookingErrors.InsufficientCredits(
                $"Insufficient available credits. Requested: {count}, Available: {AvailableCredits}."
            );
        }

        RemainingCredits -= count;
    }

    public void ConsumeReserveCredits(int count, DateTime now)
    {
        if (IsExpired(now))
        {
            throw BookingErrors.PackageExpired(now);
        }

        if (ReservedCredits < count)
        {
            throw BookingErrors.InsufficientCredits(
                $"Insufficient reserved credits. Requested: {count}, Available: {ReservedCredits}."
            );
        }

        if (RemainingCredits < count)
        {
            throw BookingErrors.InsufficientCredits(
                $"Insufficient remaining credits. Requested: {count}, Remaining: {RemainingCredits}."
            );
        }

        ReservedCredits -= count;
        RemainingCredits -= count;
    }

    public bool HasEnoughCredit()
    {
        return AvailableCredits >= 1;
    }

    public bool IsExpired(DateTime now)
    {
        return now >= ExpiresAt;
    }

    public void ReservesCredits(int count, DateTime now)
    {
        if (IsExpired(now))
        {
            throw BookingErrors.PackageExpired(now);
        }

        if (AvailableCredits < count)
        {
            throw BookingErrors.InsufficientCredits(
                $"Insufficient available credits. Requested: {count}, Available: {AvailableCredits}."
            );
        }

        ReservedCredits += count;
    }
}
