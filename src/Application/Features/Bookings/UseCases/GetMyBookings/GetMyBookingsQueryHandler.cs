using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Features.Bookings.Mappers;
using Contract.Bookings;
using Core.Exception.NetworkException;
using Core.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Bookings.UseCases.GetMyBookings;

public class GetMyBookingsQueryHandler(IBookingRepository bookings, ICurrentUserService currentUser)
    : IQueryHandler<GetMyBookingsQuery, ICollection<BookingResponse>>
{
    public async Task<Result<ICollection<BookingResponse>>> Handle(
        GetMyBookingsQuery request,
        CancellationToken cancellationToken
    )
    {
        var customerId = currentUser.CustomerId ?? throw new AuthRequiredException();

        var mine = await bookings.GetRangeAsync(
            alterQuery: query =>
                query
                    .AsNoTracking()
                    .Where(booking => booking.CustomerId == customerId)
                    .OrderByDescending(booking => booking.BookedAt),
            cancellationToken: cancellationToken
        );

        ICollection<BookingResponse> response =
        [
            .. mine.Select(booking => booking.ToBookingResponse()),
        ];

        return Result<ICollection<BookingResponse>>.Success(response);
    }
}
