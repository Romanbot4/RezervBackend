using Contract.Common;
using Core.Failure;

namespace Application.Features.Common.Mappers;

public static class FailureMapper
{
    public static FailureResponse ToFailureResponse(this Failure failure)
    {
        return new FailureResponse(
            Status: failure.Status,
            Code: failure.Code,
            Message: failure.Message,
            Errors:
            [
                .. failure.Errors?.Select(error => new FailureFieldResponse(
                    Field: error.Field,
                    Errors: error.Errors
                )) ?? [],
            ]
        );
    }
}
