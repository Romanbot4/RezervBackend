namespace Core.Exception.NetworkException;

public class ForbiddenException(string message = "You are not authorize to access this resource")
    : NetworkException(StatusCodes.Forbidden, ErrorCodes.Forbidden, message);
