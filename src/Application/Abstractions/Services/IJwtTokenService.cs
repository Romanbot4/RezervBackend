using System.Security.Claims;

namespace Application.Abstractions.Services;

public interface IJwtTokenService
{
    GeneratedToken CreateToken(TokenPayload user);

    Task<GeneratedToken> RefreshTokenAsync(string token);

    Task<TokenPayload> ValidateRefreshToken(string token);

    Task<TokenPayload> ValidateAccessToken(string token);
}

public record TokenPayload(Guid Id, string Name, string Email);

public record GeneratedToken(string AccessToken, string RefreshToken);
