using Core.Primitives.Messages;

namespace Core.Exception.NetworkException;

using Failure;

public class ValidationException(string message, ICollection<ValidationErrorMessage> errors)
    : NetworkException(StatusCodes.BadRequest, ErrorCodes.BadRequest, message, errors)
{
    public override Failure ToFailure()
    {
        return new Failure(Status, Code, Message, Errors);
    }
}
