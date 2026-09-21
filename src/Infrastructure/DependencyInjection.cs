using Application.Abstractions.DateTime;
using Application.Abstractions.Services;
using Application.Configurations;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MySql;
using Infrastructure.Common.Caching;
using Infrastructure.Common.ExceptionHandlers;
using Infrastructure.Common.Jobs;
using Infrastructure.Common.Locking;
using Infrastructure.Implementations.DateTime;
using Infrastructure.Implementations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
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
        services.AddBackgroundJobs(configuration);

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var hangfire =
            configuration
                .GetSection(HangfireConfigurations.SettingKey)
                .Get<HangfireConfigurations>()
            ?? new HangfireConfigurations();

        var connectionString = new MySqlConnectionStringBuilder(
            configuration.GetConnectionString("DefaultConnection")!
        )
        {
            AllowUserVariables = true,
            UseXaTransactions = false,
        }.ConnectionString;

        var storage = new MySqlStorage(
            connectionString,
            new MySqlStorageOptions
            {
                TablesPrefix = hangfire.TablesPrefix,
                PrepareSchemaIfNecessary = true,
                TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(hangfire.QueuePollIntervalInSeconds),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
            }
        );

        services.AddHangfire(options =>
            options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(storage)
        );

        services.AddHangfireServer(options => options.WorkerCount = hangfire.WorkerCount);

        services.AddScoped<WaitlistExpiryJob>();

        services.AddSingleton<IDashboardAuthorizationFilter, AllowAllDashboardFilter>();

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
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
