using Application.Features.TimetableSchedule.UseCases.GetTimetableSchedules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[Route("api/timetable")]
public class TimetableController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTimetableSchedules(
        [FromQuery] Guid? businessId,
        [FromQuery] DateOnly? date,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(
            new GetTimetableSchedulesQuery(businessId, date),
            cancellationToken
        );

        return Respond(result);
    }
}
