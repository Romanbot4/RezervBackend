namespace Core.Exception.NetworkException;

public class AuthInvalidCredential(string message = "Authentication credentials are invalid.")
    : NetworkException(StatusCodes.AuthError, ErrorCodes.AuthInvalidCredential, message);
