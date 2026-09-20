namespace Core.Exception.NetworkException;

using Core.Primitives.Messages;
using Failure;

public abstract class NetworkException(
    int status,
    string code,
    string message,
    ICollection<ValidationErrorMessage>? errors = null
) : CoreException(status, code, message, errors)
{
    public override Failure ToFailure()
    {
        return new Failure(Status, Code, Message, Errors);
    }
}
