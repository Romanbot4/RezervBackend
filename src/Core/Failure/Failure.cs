using Core.Primitives.Messages;

namespace Core.Failure;

public class Failure(
    int status,
    string code,
    string message,
    ICollection<ValidationErrorMessage>? errors = null
)
{
    public int Status { get; } = status;
    public string Code { get; } = code;
    public string Message { get; } = message;
    public ICollection<ValidationErrorMessage>? Errors { get; } = errors;
}
