using Application.Abstractions.Database;
using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface ICreditTransactionRepository : IGenericRepository<CreditTransactionEntity>;
