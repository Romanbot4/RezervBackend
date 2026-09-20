using Application.Abstractions.Repositories;
using Application.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class BookingRepository(IDbContext dbContext)
    : GenericRepository<BookingEntity>(dbContext),
        IBookingRepository
{
    public Task<bool> HasActiveBookingAsync(
        Guid customerId,
        Guid timetableScheduleId,
        CancellationToken cancellationToken
    )
    {
        return _query
            .AsNoTracking()
            .AnyAsync(
                b => b.CustomerId == customerId && b.TimetableScheduleId == timetableScheduleId,
                cancellationToken
            );
    }

    public Task<bool> HasOverlappingBookingAsync(
        Guid customerId,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken
    )
    {
        return _query
            .AsNoTracking()
            .Where(b => b.CustomerId == customerId)
            .AnyAsync(
                b =>
                    b.TimetableSchedule.StartTime < endTime
                    && startTime < b.TimetableSchedule.EndTime,
                cancellationToken
            );
    }
}
