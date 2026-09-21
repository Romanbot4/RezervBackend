using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Features.Business.Mappers;
using Contract.Business;
using Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Business.UseCases.GetBusinesses;

public class GetBusinessesQueryHandler(IBusinessRepository businesses)
    : IQueryHandler<GetBusinessesQuery, ICollection<BusinessResponse>>
{
    public async Task<Result<ICollection<BusinessResponse>>> Handle(
        GetBusinessesQuery request,
        CancellationToken cancellationToken
    )
    {
        var all = await businesses.GetRangeAsync(
            alterQuery: query => query.AsNoTracking().OrderBy(business => business.Name),
            cancellationToken: cancellationToken
        );

        ICollection<BusinessResponse> response =
        [
            .. all.Select(business => business.ToBusinessResponse()),
        ];

        return Result<ICollection<BusinessResponse>>.Success(response);
    }
}
