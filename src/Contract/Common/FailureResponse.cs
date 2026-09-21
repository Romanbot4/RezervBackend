namespace Contract.Common;

public record FailureResponse(
    int Status,
    string Code,
    string Message,
    ICollection<FailureFieldResponse>? Errors
);
