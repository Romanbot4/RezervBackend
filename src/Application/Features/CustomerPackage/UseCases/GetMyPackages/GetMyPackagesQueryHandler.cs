using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Features.CustomerPackage.Mappers;
using Contract.CustomerPackage;
using Core.Exception.NetworkException;
using Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CustomerPackage.UseCases.GetMyPackages;

public class GetMyPackagesQueryHandler(
    ICustomerPackageRepository customerPackages,
    ICurrentUserService currentUser
) : IQueryHandler<GetMyPackagesQuery, ICollection<CustomerPackageResponse>>
{
    public async Task<Result<ICollection<CustomerPackageResponse>>> Handle(
        GetMyPackagesQuery request,
        CancellationToken cancellationToken
    )
    {
        var customerId = currentUser.CustomerId ?? throw new AuthRequiredException();

        var owned = await customerPackages.GetRangeAsync(
            alterQuery: query =>
                query
                    .AsNoTracking()
                    .Where(customerPackage => customerPackage.CustomerId == customerId)
                    .OrderByDescending(customerPackage => customerPackage.PurchasedAt),
            cancellationToken: cancellationToken
        );

        ICollection<CustomerPackageResponse> response =
        [
            .. owned.Select(customerPackage => customerPackage.ToCustomerPackageResponse()),
        ];

        return Result<ICollection<CustomerPackageResponse>>.Success(response);
    }
}
