using Application.Abstractions.Messaging;
using Contract.CustomerPackage;

namespace Application.Features.CustomerPackage.UseCases.PurchasePackage;

public record PurchasePackageCommand(Guid PackageId) : ICommand<CustomerPackageResponse>;
