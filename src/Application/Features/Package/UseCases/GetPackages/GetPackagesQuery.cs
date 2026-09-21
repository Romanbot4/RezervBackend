using Application.Abstractions.Messaging;
using Contract.Package;

namespace Application.Features.Package.UseCases.GetPackages;

public record GetPackagesQuery(Guid? BusinessId) : IQuery<ICollection<PackageResponse>>;
