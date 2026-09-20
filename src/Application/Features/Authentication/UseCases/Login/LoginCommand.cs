using Contract.Authentication;
using Core.Primitives.Result;
using MediatR;

namespace Application.Features.Authentication.UseCases.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;
