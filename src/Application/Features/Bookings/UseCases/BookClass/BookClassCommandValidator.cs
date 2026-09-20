using Application.Features.Bookings.UseCases.BookClass;
using FluentValidation;

namespace Application.Features.Bookings.BookClass;

public class BookClassCommandValidator : AbstractValidator<BookClassCommand>
{
    public BookClassCommandValidator()
    {
        RuleFor(c => c.ScheduleId).NotEmpty();
        RuleFor(c => c.CustomerPackageId).NotEmpty();
    }
}
