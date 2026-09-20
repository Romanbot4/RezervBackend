using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Persistence.Common;

namespace Persistence.Repositories;

public class BookingRepository(IDbContext dbContext)
    : GenericRepository<BookingEntity>(dbContext),
        IBookingRepository { }
