using Application.Abstractions.Services;
using Contract.CustomerPackage;
using Domain.Entities;

namespace Application.Features.CustomerPackage.Mappers;

public static class CustomerPackageMapper
{
    public static CustomerPackageResponse ToCustomerPackageResponse(
        this CustomerPackageEntity customerPackage
    )
    {
        return new CustomerPackageResponse(
            Id: customerPackage.Id,
            CustomerId: customerPackage.CustomerId,
            PackageId: customerPackage.PackageId,
            BusinessId: customerPackage.BusinessId,
            TotalCredits: customerPackage.TotalCredits,
            RemainingCredits: customerPackage.RemainingCredits,
            ReservedCredits: customerPackage.ReservedCredits,
            PurchasedAt: customerPackage.PurchasedAt,
            ExpiresAt: customerPackage.ExpiresAt
        );
    }
}
