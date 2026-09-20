using System;
using System.Threading.Tasks;
using Application.Abstractions.DateTime;
using Application.Abstractions.Services;
using Application.Configurations;
using Core.Exception.NetworkException;
using Infrastructure.Implementations.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace UnitTests.Services;

public class JwtTokenServiceTests
{
    private readonly Mock<IDateTime> _dateTimeMock;
    private readonly JwtConfigurations _jwtConfigurations;
    private readonly JwtTokenService _sut; // System Under Test

    public JwtTokenServiceTests()
    {
        _dateTimeMock = new Mock<IDateTime>();

        // Mock current time
        _dateTimeMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

        // HMAC-SHA256 requires keys to be at least 256 bits (32 characters)
        _jwtConfigurations = new JwtConfigurations
        {
            AccessTokenKey = "SuperSecretAccessTokenKeyThatIsAtLeast32Chars!",
            RefreshTokenKey = "SuperSecretRefreshTokenKeyThatIsAtLeast32Chars!",
            AccesTokenLifeSpanInMinutes = 15,
            RefreshTokenLifeSpanInMinutes = 10080, // 7 days
        };

        var options = Options.Create(_jwtConfigurations);
        _sut = new JwtTokenService(options, _dateTimeMock.Object);
    }

    [Fact]
    public void CreateToken_ShouldIssueValidAccessAndRefreshTokens()
    {
        // Arrange
        var payload = new TokenPayload(Guid.NewGuid(), "Test User", "test@example.com");

        // Act
        var result = _sut.CreateToken(payload);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
    }

    [Fact]
    public async Task ValidateAccessToken_WithValidToken_ShouldReturnCorrectTokenPayload()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var payload = new TokenPayload(expectedId, "Test User", "test@example.com");
        var generatedToken = _sut.CreateToken(payload);

        // Act
        var result = await _sut.ValidateAccessToken(generatedToken.AccessToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal("Test User", result.Name);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task RefreshTokenAsync_WithValidRefreshToken_ShouldIssueNewTokens()
    {
        // Arrange
        var payload = new TokenPayload(Guid.NewGuid(), "Refresh User", "refresh@example.com");
        var initialTokens = _sut.CreateToken(payload);

        // Act
        var newTokens = await _sut.RefreshTokenAsync(initialTokens.RefreshToken);

        // Assert
        Assert.NotNull(newTokens);
        Assert.False(string.IsNullOrWhiteSpace(newTokens.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(newTokens.RefreshToken));

        // Ensure new tokens are distinct (assuming time has passed or entropy exists,
        // though in strict unit tests they might match if generated in the exact same millisecond.
        // We primarily care that they are issued successfully).
    }

    [Fact]
    public async Task ValidateAccessToken_WithExpiredToken_ShouldThrowAuthTokenExpiredException()
    {
        // Arrange
        // Simulate token creation in the past by mocking IDateTime to a past date
        var pastDate = DateTime.UtcNow.AddMinutes(-60);
        _dateTimeMock.Setup(d => d.UtcNow).Returns(pastDate);

        var payload = new TokenPayload(Guid.NewGuid(), "Expired User", "expired@example.com");

        // This token is created with an 'exp' claim set to (pastDate + 15 mins), which is still in the past.
        var expiredToken = _sut.CreateToken(payload);

        // Reset the mock to current time (optional, as JsonWebTokenHandler uses system time for validation by default)
        _dateTimeMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AuthTokenExpiredException>(() =>
            _sut.ValidateAccessToken(expiredToken.AccessToken)
        );

        Assert.Equal("Token expired", exception.Message);
    }

    [Fact]
    public async Task ValidateRefreshToken_WithExpiredToken_ShouldThrowAuthTokenExpiredException()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddDays(-10); // Past the 7-day refresh lifespan
        _dateTimeMock.Setup(d => d.UtcNow).Returns(pastDate);

        var payload = new TokenPayload(
            Guid.NewGuid(),
            "Expired Refresh User",
            "expired@example.com"
        );
        var tokens = _sut.CreateToken(payload);

        // Act & Assert
        await Assert.ThrowsAsync<AuthTokenExpiredException>(() =>
            _sut.RefreshTokenAsync(tokens.RefreshToken)
        );
    }

    [Theory]
    [InlineData("not.a.real.token")]
    [InlineData("eyJh.InvalidPayload.Sig")]
    [InlineData("")]
    public async Task ValidateAccessToken_WithMalformedToken_ShouldThrowAuthTokenInvalidException(
        string invalidToken
    )
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<AuthTokenInvalidException>(() =>
            _sut.ValidateAccessToken(invalidToken)
        );

        Assert.Equal("Invalid token", exception.Message);
    }

    [Fact]
    public async Task ValidateAccessToken_WithValidTokenButWrongKey_ShouldThrowAuthTokenInvalidException()
    {
        // Arrange
        var payload = new TokenPayload(Guid.NewGuid(), "Wrong Key User", "wrongkey@example.com");

        // Generate a token using the Refresh Key instead of the Access Key
        var tokenWithWrongKey = _sut.Encode(
            _jwtConfigurations.RefreshTokenKey,
            15,
            new System.Security.Claims.ClaimsIdentity()
        );

        // Act & Assert
        // Trying to validate it as an Access Token should fail because the signing keys don't match
        await Assert.ThrowsAsync<AuthTokenInvalidException>(() =>
            _sut.ValidateAccessToken(tokenWithWrongKey)
        );
    }
}
