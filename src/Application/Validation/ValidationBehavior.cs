using Core.Primitives.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Validation;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var context = new ValidationContext<TRequest>(request);

        List<ValidationFailure> failures =
        [
            .. _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null),
        ];

        if (failures.Count != 0)
        {
            throw new Core.Exception.NetworkException.ValidationException(
                "Request is not valid",
                [
                    .. failures.Select(e => new ValidationErrorMessage(
                        e.PropertyName,
                        [e.ErrorMessage]
                    )),
                ]
            );
        }

        return await next();
    }
}
