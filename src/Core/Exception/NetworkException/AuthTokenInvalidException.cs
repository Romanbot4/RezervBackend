namespace Core.Exception.NetworkException;

public class AuthTokenInvalidException(string message = "Token is invalid. Login again.")
    : NetworkException(StatusCodes.AuthError, ErrorCodes.TokenInvalid, message);
