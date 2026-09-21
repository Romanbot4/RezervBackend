using Contract.Business;
using Domain.Entities;

namespace Application.Features.Business.Mappers;

public static class BusinessMapper
{
    public static BusinessResponse ToBusinessResponse(this BusinessEntity business)
    {
        return new BusinessResponse(
            Id: business.Id,
            Name: business.Name,
            IsActive: business.IsActive
        );
    }
}
