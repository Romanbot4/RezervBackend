namespace Core.Exception.NetworkException;

public class AuthRequiredException(
    string message = "Authentication required to access this resource."
) : NetworkException(StatusCodes.AuthError, ErrorCodes.AuthInvalidCredential, message);
