using Application.Abstractions.Messaging;
using Contract.Business;

namespace Application.Features.Business.UseCases.GetBusinesses;

public record GetBusinessesQuery : IQuery<ICollection<BusinessResponse>>;
