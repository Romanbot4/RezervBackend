namespace Core.Exception.NetworkException;

public class UnknownException(string message = "Something went wrong")
    : NetworkException(StatusCodes.Unknown, ErrorCodes.Unknown, message);
