namespace Core.Exception.NetworkException;

public class AuthTokenExpiredException(string message = "Token has expired. Refresh and try again.")
    : NetworkException(StatusCodes.AuthError, ErrorCodes.TokenExpired, message);
