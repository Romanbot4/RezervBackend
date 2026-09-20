using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Persistence.Common;

namespace Persistence.Repositories;

public class BusinessRepository(IDbContext dbContext)
    : GenericRepository<BusinessEntity>(dbContext),
        IBusinessRepository;
