using Application.Abstractions.Database;

namespace Application.Abstractions.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

public interface IBookingRepository : IGenericRepository<BookingEntity>
{
    Task<bool> HasActiveBookingAsync(Guid customerId, Guid id, CancellationToken cancellationToken);

    Task<bool> HasOverlappingBookingAsync(
        Guid customerId,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken
    );
}
