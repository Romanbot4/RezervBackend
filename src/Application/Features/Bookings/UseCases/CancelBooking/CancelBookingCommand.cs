using Application.Abstractions.Messaging;
using Contract.Bookings;

namespace Application.Features.Bookings.UseCases.CancelBooking;

public record CancelBookingCommand(Guid BookingId) : ICommand<BookClassResponse>;
