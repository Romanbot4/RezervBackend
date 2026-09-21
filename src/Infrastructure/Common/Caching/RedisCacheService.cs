namespace Infrastructure.Common.Caching;

using System;
using System.Text.Json;
using Application.Abstractions.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

public class RedisCacheService(IConnectionMultiplexer connection, ILogger<RedisCacheService> logger)
    : ICacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(
        JsonSerializerDefaults.Web
    );

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cached = await connection.GetDatabase().StringGetAsync(key);

            return cached.HasValue
                ? JsonSerializer.Deserialize<T>(cached!, SerializerOptions)
                : default;
        }
        catch (RedisException exception)
        {
            logger.LogWarning(exception, "Cache read failed for {Key}, reading the database", key);
            return default;
        }
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiry,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await connection
                .GetDatabase()
                .StringSetAsync(key, JsonSerializer.Serialize(value, SerializerOptions), expiry);
        }
        catch (RedisException exception)
        {
            logger.LogWarning(exception, "Cache write failed for {Key}", key);
        }
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan expiry,
        CancellationToken cancellationToken = default
    )
    {
        var cached = await GetAsync<T>(key, cancellationToken);

        if (cached is not null)
        {
            return cached;
        }

        var value = await factory(cancellationToken);

        await SetAsync(key, value, expiry, cancellationToken);

        return value;
    }

    public async Task RemoveByPrefixAsync(
        string prefix,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var database = connection.GetDatabase();

            foreach (var endpoint in connection.GetEndPoints())
            {
                var server = connection.GetServer(endpoint);

                if (!server.IsConnected || server.IsReplica)
                {
                    continue;
                }

                await foreach (
                    var key in server
                        .KeysAsync(database.Database, $"{prefix}*")
                        .WithCancellation(cancellationToken)
                )
                {
                    await database.KeyDeleteAsync(key);
                }
            }
        }
        catch (RedisException exception)
        {
            logger.LogWarning(exception, "Cache eviction failed for prefix {Prefix}", prefix);
        }
    }
}
