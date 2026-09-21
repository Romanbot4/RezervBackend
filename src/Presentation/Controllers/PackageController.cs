using Application.Features.CustomerPackage.UseCases.GetMyPackages;
using Application.Features.Package.UseCases.GetPackages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[Route("api/packages")]
public class PackageController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPackages(
        [FromQuery] Guid? businessId,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(new GetPackagesQuery(businessId), cancellationToken);

        return Respond(result);
    }

    [HttpGet("mine")]
    [Authorize]
    public async Task<IActionResult> GetMyPackages(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetMyPackagesQuery(), cancellationToken);

        return Respond(result);
    }
}
