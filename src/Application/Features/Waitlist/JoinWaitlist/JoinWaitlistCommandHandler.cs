using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Features.CustomerPackage.Mappers;
using Application.Features.WaitList.Mappers;
using Contract.Waitlist;
using Core.Primitives.Result;

namespace Application.Features.Waitlist.JoinWaitlist;

public class JoinWaitlistCommandHandler(IBookingService booking)
    : ICommandHandler<JoinWaitlistCommand, JoinWaitlistResponse>
{
    public async Task<Result<JoinWaitlistResponse>> Handle(
        JoinWaitlistCommand request,
        CancellationToken cancellationToken
    )
    {
        var context = await booking.ResolveAsync(
            request.ScheduleId,
            request.CustomerPackageId,
            cancellationToken
        );

        var entry = await booking.JoinWaitlistAsync(context, cancellationToken);

        return Result<JoinWaitlistResponse>.Success(
            new JoinWaitlistResponse(
                Waitlist: entry.ToWaitlistResponse(),
                CustomerPackage: context.CustomerPackage.ToCustomerPackageResponse()
            )
        );
    }
}
