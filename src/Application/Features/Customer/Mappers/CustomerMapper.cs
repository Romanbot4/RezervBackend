using Application.Abstractions.Services;
using Contract.Customer;
using Domain.Entities;

namespace Application.Features.Customer.Mappers;

public static class CustomerMapper
{
    public static TokenPayload ToTokenPayload(this CustomerEntity customer)
    {
        return new TokenPayload(Id: customer.Id, Name: customer.Name, Email: customer.Email);
    }

    public static CustomerResponse ToCustomerResponse(this CustomerEntity customer)
    {
        return new CustomerResponse(Id: customer.Id, Name: customer.Name, Email: customer.Email);
    }
}
