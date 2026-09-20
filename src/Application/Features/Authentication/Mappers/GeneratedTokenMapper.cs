using Application.Abstractions.Services;

namespace Application.Features.Authentication.Mappers;

public static class GeneratedTokenMapper
{
    public static TokenResponse ToTokenResponse(this GeneratedToken Token)
    {
        return new TokenResponse(Token.AccessToken, Token.RefreshToken);
    }
}
