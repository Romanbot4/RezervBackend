namespace Core.Exception.NetworkException;

public class UnprocessableException(string message = "You action cannot be processed.")
    : NetworkException(StatusCodes.Unprocessable, ErrorCodes.Unprocessable, message);
