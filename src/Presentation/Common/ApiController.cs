using Application.Features.Common.Mappers;
using Core.Primitives.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common;

[ApiController]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected IActionResult Respond<TValue>(Result<TValue> result)
    {
        return Respond(result, StatusCodes.Status200OK);
    }

    protected IActionResult Respond<TValue>(Result<TValue> result, int successStatusCode)
    {
        return result.Match<IActionResult>(
            onSuccess: value => StatusCode(successStatusCode, value),
            onFailure: failure => StatusCode(failure.Status, failure.ToFailureResponse())
        );
    }
}
