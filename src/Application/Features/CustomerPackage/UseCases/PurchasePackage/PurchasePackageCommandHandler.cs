using Application.Abstractions.Database;
using Application.Abstractions.DateTime;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Features.CustomerPackage.Mappers;
using Contract.CustomerPackage;
using Core.Exception.NetworkException;
using Core.Primitives.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;

namespace Application.Features.CustomerPackage.UseCases.PurchasePackage;

public class PurchasePackageCommandHandler(
    IPackageRepository packages,
    ICustomerPackageRepository customerPackages,
    ICreditTransactionRepository creditTransactions,
    ICurrentUserService currentUser,
    IDateTime dateTime,
    IUnitOfWork unitOfWork
) : ICommandHandler<PurchasePackageCommand, CustomerPackageResponse>
{
    public async Task<Result<CustomerPackageResponse>> Handle(
        PurchasePackageCommand request,
        CancellationToken cancellationToken
    )
    {
        var customerId = currentUser.CustomerId ?? throw new AuthRequiredException();

        var now = dateTime.UtcNow;

        var package =
            await packages.GetByIdAsync(request.PackageId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Package Not Found");

        if (!package.IsActive)
        {
            throw PackageErrors.PackageNotActive();
        }

        var customerPackage = package.PurchaseFor(customerId, now);

        await customerPackages.InsertAsync(customerPackage, cancellationToken);

        var creditTransaction = new CreditTransactionEntity(
            id: Guid.NewGuid(),
            customerPackageId: customerPackage.Id,
            type: CreditTransactionType.Purchase,
            amount: package.Credits,
            reason: "Package purchased",
            occurredAt: now
        );

        await creditTransactions.InsertAsync(creditTransaction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CustomerPackageResponse>.Success(customerPackage.ToCustomerPackageResponse());
    }
}
