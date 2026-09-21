using Application.Abstractions.Messaging;
using Contract.CustomerPackage;

namespace Application.Features.CustomerPackage.UseCases.GetMyPackages;

public record GetMyPackagesQuery : IQuery<ICollection<CustomerPackageResponse>>;
