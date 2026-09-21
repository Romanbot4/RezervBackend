using Application.Features.Waitlist.JoinWaitlist;
using Contract.Waitlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[Route("api/waitlist")]
[Authorize]
public class WaitlistController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<IActionResult> JoinWaitlist(
        [FromBody] JoinWaitlistRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(
            new JoinWaitlistCommand(request.ScheduleId, request.CustomerPackageId),
            cancellationToken
        );

        return Respond(result, StatusCodes.Status201Created);
    }
}
