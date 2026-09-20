using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class TimetableScheduleRepository(IDbContext dbContext)
    : GenericRepository<TimetableScheduleEntity>(dbContext),
        ITimetableScheduleRepository { }
