namespace Core.Exception.NetworkException;

public class AuthExpiredException(string message = "Token has expired. Refresh and try again.")
    : NetworkException(StatusCodes.AuthError, ErrorCodes.AuthExpired, message);
