namespace Contract.Common;

public record FailureFieldResponse(string Field, ICollection<string> Errors);
