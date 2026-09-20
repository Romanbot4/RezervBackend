namespace Core.Exception.NetworkException;

public class NotSupportedException(string message = "Server does not support this format")
    : NetworkException(StatusCodes.Unprocessable, ErrorCodes.Unprocessable, message);
