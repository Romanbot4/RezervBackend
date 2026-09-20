using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Services;

namespace Infrastructure.Implementations.Services;

public class Md5HashPasswordService : IHashPasswordService
{
    public string Hash(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashedBytes = MD5.HashData(bytes);
        return Convert.ToBase64String(hashedBytes);
    }

    public bool Verify(string password, string passwordHash)
    {
        return Hash(password) == passwordHash;
    }
}
