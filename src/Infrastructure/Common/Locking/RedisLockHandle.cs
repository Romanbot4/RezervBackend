using Application.Abstractions.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Common.Locking;

public sealed class RedisLockHandle(IDatabase database, string key, string token, ILogger logger)
    : ILockHandle
{
    private const string ReleaseScript = """
        if redis.call('GET', KEYS[1]) == ARGV[1] then
            return redis.call('DEL', KEYS[1])
        else
            return 0
        end
        """;

    public string Key => key;

    public async ValueTask DisposeAsync()
    {
        try
        {
            var result = await database.ScriptEvaluateAsync(ReleaseScript, [key], [token]);

            if (result is { IsNull: false } && (int)result == 1)
            {
                return;
            }

            logger.LogWarning("Lock {Key} already expired before it could be released", key);
        }
        catch (System.Exception exception)
        {
            logger.LogWarning(exception, "Could not release lock {Key}", key);
        }
    }
}
