using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ICustomerRepository : IGenericRepository<CustomerEntity>
{
    Task<CustomerEntity> FindByEmailAsync(string email, CancellationToken cancellationToken);
}
