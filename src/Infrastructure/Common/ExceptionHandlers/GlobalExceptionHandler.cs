using Application.Features.Common.Mappers;
using Core.Exception;
using Core.Exception.NetworkException;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common.ExceptionHandlers;

internal class GlobalExceptionHandler : IExceptionHandler
{
    internal static async Task HandleException(HttpContext context, System.Exception exception)
    {
        var coreException = exception switch
        {
            CoreException e => e,
            _ => new UnknownException("Internal server error"),
        };

        var response = coreException.ToFailure().ToFailureResponse();

        context.Response.StatusCode = response.Status;

        await context.Response.WriteAsJsonAsync(response);
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken
    )
    {
        await HandleException(httpContext, exception);

        return true;
    }
}
