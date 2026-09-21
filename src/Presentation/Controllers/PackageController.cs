using Application.Features.CustomerPackage.UseCases.GetMyPackages;
using Application.Features.CustomerPackage.UseCases.PurchasePackage;
using Application.Features.Package.UseCases.GetPackages;
using Contract.CustomerPackage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpPost("purchase")]
    [Authorize]
    public async Task<IActionResult> PurchasePackage(
        [FromBody] PurchasePackageRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(
            new PurchasePackageCommand(request.PackageId),
            cancellationToken
        );

        return Respond(result, StatusCodes.Status201Created);
    }
}
