using FluentValidation;

namespace Application.Features.CustomerPackage.UseCases.PurchasePackage;

public class PurchasePackageCommandValidator : AbstractValidator<PurchasePackageCommand>
{
    public PurchasePackageCommandValidator()
    {
        RuleFor(c => c.PackageId).NotEmpty();
    }
}
