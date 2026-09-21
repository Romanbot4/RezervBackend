namespace Infrastructure.Common.Locking;

using System;
using Application.Abstractions.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

public class RedisDistributedLockService(
    IConnectionMultiplexer connection,
    ILogger<RedisDistributedLockService> logger
) : IDistributedLockService
{
    private static readonly TimeSpan InitialRetryDelay = TimeSpan.FromMilliseconds(15);

    private static readonly TimeSpan MaxRetryDelay = TimeSpan.FromMilliseconds(120);

    public async Task<ILockHandle?> AcquireAsync(
        string key,
        TimeSpan ttl,
        TimeSpan wait,
        CancellationToken cancellationToken = default
    )
    {
        var database = connection.GetDatabase();
        var token = Guid.NewGuid().ToString("N");
        var deadline = DateTime.UtcNow + wait;
        var delay = InitialRetryDelay;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool acquired;

            try
            {
                acquired = await database.StringSetAsync(
                    key,
                    token,
                    expiry: ttl,
                    keepTtl: false,
                    when: When.NotExists
                );
            }
            catch (RedisException exception)
            {
                logger.LogError(
                    exception,
                    "Redis is unreachable so {Key} is proceeding without a lock. Capacity and credits are still enforced by the conditional updates in MySql",
                    key
                );

                return new UnlockedHandle(key);
            }

            if (acquired)
            {
                return new RedisLockHandle(database, key, token, logger);
            }

            if (DateTime.UtcNow >= deadline)
            {
                logger.LogWarning("Timed out waiting {Wait} for lock {Key}", wait, key);
                return null;
            }

            await Task.Delay(WithJitter(delay), cancellationToken);

            delay = TimeSpan.FromMilliseconds(
                Math.Min(MaxRetryDelay.TotalMilliseconds, delay.TotalMilliseconds * 2)
            );
        }
    }

    private static TimeSpan WithJitter(TimeSpan delay) =>
        delay + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 15));
}
