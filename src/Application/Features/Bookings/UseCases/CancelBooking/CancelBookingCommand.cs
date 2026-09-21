using Contract.Bookings;
using Core.Primitives.Result;
using MediatR;

namespace Application.Features.Bookings.UseCases.CancelBooking;

public record CancelBookingCommand(Guid BookingId) : IRequest<Result<BookClassResponse>>;
