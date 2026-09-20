namespace Contract.CustomerPackage;

public record CustomerPackageResponse(
    Guid Id,
    Guid CustomerId,
    Guid PackageId,
    Guid BusinessId,
    int TotalCredits,
    int RemainingCredits,
    int ReservedCredits,
    DateTime PurchasedAt,
    DateTime ExpiresAt
);
