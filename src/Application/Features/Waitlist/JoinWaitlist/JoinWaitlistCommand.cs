using Application.Abstractions.Messaging;
using Contract.Waitlist;

namespace Application.Features.Waitlist.JoinWaitlist;

public record JoinWaitlistCommand(Guid ScheduleId, Guid CustomerPackageId)
    : ICommand<JoinWaitlistResponse>;
