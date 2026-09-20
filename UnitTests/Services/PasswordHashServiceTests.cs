using System.Security.Cryptography;
using System.Text;
using Infrastructure.Implementations.Services;
using Xunit;

namespace UnitTests.Services;

public class Md5HashPasswordServiceTests
{
    private readonly Md5HashPasswordService _sut;

    public Md5HashPasswordServiceTests()
    {
        _sut = new Md5HashPasswordService();
    }

    [Fact]
    public void Hash_ShouldReturnExpectedBase64Md5Hash()
    {
        // Arrange
        var password = "TestPassword123";

        // Manually compute the expected Base64 MD5 hash for comparison
        var expectedBytes = MD5.HashData(Encoding.UTF8.GetBytes(password));
        var expectedHash = Convert.ToBase64String(expectedBytes);

        // Act
        var actualHash = _sut.Hash(password);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void Verify_ShouldReturnTrue_WhenPasswordMatchesHash()
    {
        // Arrange
        var password = "SecurePassword!";
        var hash = _sut.Hash(password);

        // Act
        var result = _sut.Verify(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
    {
        // Arrange
        var password = "SecurePassword!";
        var wrongPassword = "WrongPassword!";
        var hash = _sut.Hash(password);

        // Act
        var result = _sut.Verify(wrongPassword, hash);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("password1", "password2")]
    [InlineData("Admin123", "admin123")]
    [InlineData("", "notempty")]
    public void Hash_ShouldReturnDifferentHashes_ForDifferentPasswords(string pass1, string pass2)
    {
        // Act
        var hash1 = _sut.Hash(pass1);
        var hash2 = _sut.Hash(pass2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }
}
