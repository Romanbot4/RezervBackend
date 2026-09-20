using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Persistence.Common;

namespace Persistence.Repositories;

public class CreditTransactionRepository(IDbContext dbContext)
    : GenericRepository<CreditTransactionEntity>(dbContext),
        ICreditTransactionRepository;
