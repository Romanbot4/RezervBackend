using Application.Abstractions.Messaging;
using Contract.Authentication;

namespace Application.Features.Authentication.UseCases.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
