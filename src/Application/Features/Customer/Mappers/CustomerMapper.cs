using Application.Abstractions.Services;
using Contract.Customer;
using Domain.Entities;

namespace Application.Features.Customer.Mappers;

public static class CustomerMapper
{
    public static TokenPayload ToTokenPayload(this CustomerEntity user)
    {
        return new TokenPayload(Id: user.Id, Name: user.Name, Email: user.Email);
    }

    public static CustomerResponse ToCustomerResponse(this CustomerEntity user)
    {
        return new CustomerResponse(Id: user.Id, Name: user.Name, Email: user.Email);
    }
}
