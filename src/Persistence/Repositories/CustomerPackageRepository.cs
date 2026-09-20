using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Persistence.Common;

namespace Persistence.Repositories;

public class CustomerPackageRepository(IDbContext dbContext)
    : GenericRepository<CustomerPackageEntity>(dbContext),
        ICustomerPackageRepository { }
