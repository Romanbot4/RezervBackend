using Contract.Package;
using Domain.Entities;

namespace Application.Features.Package.Mappers;

public static class PackageMapper
{
    public static PackageResponse ToPackageResponse(this PackageEntity package)
    {
        return new PackageResponse(
            Id: package.Id,
            BusinessId: package.BusinessId,
            BusinessName: package.Business.Name,
            Name: package.Name,
            Credits: package.Credits,
            ValidityDays: package.ValidityDays,
            Price: package.Price
        );
    }
}
