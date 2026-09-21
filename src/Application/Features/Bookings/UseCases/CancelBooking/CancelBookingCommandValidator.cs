using Application.Features.Bookings.UseCases.CancelBooking;
using FluentValidation;

namespace Application.Features.Bookings.CancelBooking;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();
    }
}
