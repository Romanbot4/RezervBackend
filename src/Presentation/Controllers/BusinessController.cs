using Application.Features.Business.UseCases.GetBusinesses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[Route("api/businesses")]
public class BusinessController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBusinesses(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetBusinessesQuery(), cancellationToken);

        return Respond(result);
    }
}
