using Application.Abstractions.Messaging;
using Contract.Bookings;

namespace Application.Features.Bookings.UseCases.GetMyBookings;

public record GetMyBookingsQuery : IQuery<ICollection<BookingResponse>>;
