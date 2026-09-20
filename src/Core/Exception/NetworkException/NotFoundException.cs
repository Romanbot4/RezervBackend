namespace Core.Exception.NetworkException;

public class NotFoundException(string message = "Content Not Found")
    : NetworkException(StatusCodes.NotFound, ErrorCodes.NotFound, message);
