namespace Core.Primitives.Messages;

public class ValidationErrorMessage(string field, ICollection<string> errors)
{
    public string Field { get; } = field;
    public ICollection<string> Errors { get; } = errors;
}
