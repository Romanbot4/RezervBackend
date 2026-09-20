using Application.Abstractions.DateTime;
using Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                options => options.CommandTimeout(60)
            )
        );

        services.AddScoped<IDbContext, ApplicationDbContext>(services =>
            services.GetRequiredService<ApplicationDbContext>()
        );
        services.AddScoped<IUnitOfWork, ApplicationDbContext>(services =>
            services.GetRequiredService<ApplicationDbContext>()
        );
        return services;
    }
}
