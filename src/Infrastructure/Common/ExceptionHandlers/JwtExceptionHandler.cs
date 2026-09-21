using Core.Exception.NetworkException;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infrastructure.Common.ExceptionHandlers;

public static class JwtExceptionHandler
{
    public static Task OnChallenge(JwtBearerChallengeContext context)
    {
        context.HandleResponse();

        try
        {
            if (context.ErrorDescription?.StartsWith("The token expired") == true)
            {
                throw new AuthTokenExpiredException();
            }

            if (string.IsNullOrEmpty(context.Error))
            {
                throw new AuthRequiredException();
            }

            throw new AuthTokenInvalidException();
        }
        catch (System.Exception exception)
        {
            return GlobalExceptionHandler.HandleException(context.HttpContext, exception);
        }
    }
}
