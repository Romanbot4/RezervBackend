using Application.Features.Authentication.UseCases.Login;
using Contract.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[Route("api/auth")]
public class AuthController(ISender sender) : ApiController(sender)
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(
            new LoginCommand(request.Email, request.Password),
            cancellationToken
        );

        return Respond(result);
    }
}
