using Contract.Waitlist;
using Core.Primitives.Result;
using MediatR;

namespace Application.Features.Waitlist.JoinWaitlist;

public record JoinWaitlistCommand(Guid ScheduleId, Guid CustomerPackageId)
    : IRequest<Result<JoinWaitlistResponse>>;
