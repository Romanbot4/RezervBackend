using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Persistence.Common;

namespace Persistence.Repositories;

public class PackageRepository(IDbContext dbContext)
    : GenericRepository<PackageEntity>(dbContext),
        IPackageRepository { }
