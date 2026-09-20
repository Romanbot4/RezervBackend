using FluentValidation;

namespace Application.Features.Waitlist.JoinWaitlist;

public class JoinWaitlistCommandValidator : AbstractValidator<JoinWaitlistCommand>
{
    public JoinWaitlistCommandValidator()
    {
        RuleFor(c => c.ScheduleId).NotEmpty();
        RuleFor(c => c.CustomerPackageId).NotEmpty();
    }
}
