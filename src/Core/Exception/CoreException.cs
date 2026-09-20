using Core.Primitives.Messages;

namespace Core.Exception;

using Failure;

public abstract class CoreException(
    int status,
    string code,
    string message,
    ICollection<ValidationErrorMessage>? errors = null
) : System.Exception(message)
{
    public int Status { get; } = status;
    public string Code { get; } = code;
    public ICollection<ValidationErrorMessage>? Errors { get; } = errors;

    public abstract Failure ToFailure();
}
