using Application.Abstractions.DateTime;
using Application.Abstractions.Services;
using Application.Configurations;
using Infrastructure.Common.ExceptionHandlers;
using Infrastructure.Common.Locking;
using Infrastructure.Implementations.DateTime;
using Infrastructure.Implementations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IDateTime, MachineDateTime>();
        services.AddScoped<IHashPasswordService, Md5HashPasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddRedis(configuration);

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var redis =
            configuration.GetSection(RedisConfigurations.SettingKey).Get<RedisConfigurations>()
            ?? new RedisConfigurations();

        var options = ConfigurationOptions.Parse(redis.ConnectionString);
        options.AbortOnConnectFail = false;
        options.ConnectTimeout = 2_000;
        options.SyncTimeout = 2_000;

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(options));
        services.AddSingleton<IDistributedLockService, RedisDistributedLockService>();

        return services;
    }
}
