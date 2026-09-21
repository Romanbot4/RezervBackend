using Application.Abstractions.DateTime;
using Application.Abstractions.Repositories;
using Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

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

        services.AddScoped<IBusinessRepository, BusinessRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ICustomerPackageRepository, CustomerPackageRepository>();
        services.AddScoped<ITimetableScheduleRepository, TimetableScheduleRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IWaitlistRepository, WaitlistRepository>();
        services.AddScoped<ICreditTransactionRepository, CreditTransactionRepository>();

        return services;
    }
}
