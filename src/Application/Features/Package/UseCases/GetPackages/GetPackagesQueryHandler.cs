using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Features.Package.Mappers;
using Contract.Package;
using Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Package.UseCases.GetPackages;

public class GetPackagesQueryHandler(IPackageRepository packages)
    : IQueryHandler<GetPackagesQuery, ICollection<PackageResponse>>
{
    public async Task<Result<ICollection<PackageResponse>>> Handle(
        GetPackagesQuery request,
        CancellationToken cancellationToken
    )
    {
        var available = await packages.GetRangeAsync(
            alterQuery: query =>
                query
                    .AsNoTracking()
                    .Include(package => package.Business)
                    .Where(package => package.IsActive)
                    .Where(package =>
                        request.BusinessId == null || package.BusinessId == request.BusinessId
                    )
                    .OrderBy(package => package.Business.Name)
                    .ThenBy(package => package.Credits),
            cancellationToken: cancellationToken
        );

        ICollection<PackageResponse> response =
        [
            .. available.Select(package => package.ToPackageResponse()),
        ];

        return Result<ICollection<PackageResponse>>.Success(response);
    }
}
