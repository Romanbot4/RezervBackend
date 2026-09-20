using System.Security.Cryptography;
using System.Text;

namespace Persistence.Common.Helpers;

/// <summary>
/// To generate stable GUID from string
/// </summary>
public static class DeterministicGuid
{
    public static Guid From(string key)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(key));
        return new Guid(hash);
    }
}
