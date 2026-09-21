namespace Contract.Package;

public record PackageResponse(
    Guid Id,
    Guid BusinessId,
    string BusinessName,
    string Name,
    int Credits,
    int ValidityDays,
    decimal Price
);
