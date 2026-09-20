using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class WaitlistRepository(IDbContext dbContext)
    : GenericRepository<WaitlistEntryEntity>(dbContext),
        IWaitlistRepository { }
