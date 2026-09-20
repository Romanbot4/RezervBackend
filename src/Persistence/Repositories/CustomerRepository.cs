using Application.Abstractions.Repositories;
using Application.Database;
using Core.Exception.NetworkException;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class CustomerRepository(IDbContext dbContext)
    : GenericRepository<CustomerEntity>(dbContext),
        ICustomerRepository
{
    public Task<CustomerEntity> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return _query
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email, cancellationToken)!;
        }
        catch (Exception)
        {
            throw new NotFoundException($"Customer with email {email} not found");
        }
    }
}
