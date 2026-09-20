using System.Security.Claims;
using System.Text;
using Application.Abstractions.DateTime;
using Application.Abstractions.Services;
using Application.Configurations;
using Core.Exception.NetworkException;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Implementations.Services;

public class JwtTokenService(
    IOptions<JwtConfigurations> jwtConfigurationsOptions,
    IDateTime dateTime
) : IJwtTokenService
{
    private readonly JwtConfigurations jwtConfigurations = jwtConfigurationsOptions.Value;

    public GeneratedToken CreateToken(TokenPayload user)
    {
        var claims = new ClaimsIdentity([
            new Claim("sub", user.Id.ToString()),
            new Claim("name", user.Name),
            new Claim("email", user.Email),
        ]);

        var accessToken = Encode(
            jwtConfigurations.AccessTokenKey,
            jwtConfigurations.AccesTokenLifeSpanInMinutes,
            claims
        );

        var refreshToken = Encode(
            jwtConfigurations.RefreshTokenKey,
            jwtConfigurations.RefreshTokenLifeSpanInMinutes,
            claims
        );

        return new GeneratedToken(accessToken, refreshToken);
    }

    public async Task<GeneratedToken> RefreshTokenAsync(string token)
    {
        var user = await ValidateRefreshToken(token);
        return CreateToken(user);
    }

    public Task<TokenPayload> ValidateAccessToken(string token)
    {
        return ValidateToken(jwtConfigurations.AccessTokenKey, token);
    }

    public Task<TokenPayload> ValidateRefreshToken(string token)
    {
        return ValidateToken(jwtConfigurations.RefreshTokenKey, token);
    }

    public string Encode(string key, int expirationInMinutes, ClaimsIdentity claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var handler = new JsonWebTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = dateTime.UtcNow.AddMinutes(expirationInMinutes),
            IssuedAt = dateTime.UtcNow,
            SigningCredentials = credential,
        };
        return handler.CreateToken(tokenDescriptor);
    }

    private static async Task<TokenPayload> ValidateToken(string key, string token)
    {
        var result = await DecodeAsync(key, token);
        var claims = result.ClaimsIdentity;

        var id = Guid.Parse(claims.FindFirst("sub")!.Value);
        var name = claims.FindFirst("name")!.Value;
        var email = claims.FindFirst("email")!.Value;

        return new TokenPayload(Id: id, Name: name, Email: email);
    }

    public static async Task<TokenValidationResult> DecodeAsync(string key, string source)
    {
        var validationParameters = GetTokenValidationParameters(key);

        var handler = new JsonWebTokenHandler();
        var result = await handler.ValidateTokenAsync(source, validationParameters);

        if (result.IsValid)
        {
            return result;
        }

        return HandleFailedTokenValidationResult(result);
    }

    public static TokenValidationParameters GetTokenValidationParameters(string key)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            NameClaimType = "sub",
        };
    }

    public static TokenValidationResult HandleFailedTokenValidationResult(
        TokenValidationResult result
    )
    {
        if (
            result.Exception is SecurityTokenInvalidLifetimeException
            || result.Exception is SecurityTokenExpiredException
        )
        {
            throw new AuthTokenExpiredException("Token expired");
        }

        throw new AuthTokenInvalidException("Invalid token");
    }
}
