using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Features.Authentication.Mappers;
using Application.Features.Customer.Mappers;
using Contract.Authentication;
using Core.Exception.NetworkException;
using Core.Primitives.Result;

namespace Application.Features.Authentication.UseCases.Login;

public class LoginCommandHandler(
    ICustomerRepository customers,
    IHashPasswordService hashPasswordService,
    IJwtTokenService jwtTokenService
) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var customer = await customers.FindByEmailAsync(request.Email, cancellationToken);

        if (
            customer is null
            || !hashPasswordService.Verify(request.Password, customer.PasswordHash)
        )
        {
            throw new AuthTokenInvalidException();
        }

        var token = jwtTokenService.CreateToken(customer.ToTokenPayload());

        return Result<LoginResponse>.Success(
            new LoginResponse(
                Customer: customer.ToCustomerResponse(),
                Token: token.ToTokenResponse()
            )
        );
    }
}
