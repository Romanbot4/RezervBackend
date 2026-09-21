using Application.Abstractions.Services;
using Application.Configurations;
using Application.Services;
using Application.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<JwtConfigurations>(
            configuration.GetSection(JwtConfigurations.SettingKey)
        );

        services.Configure<HangfireConfigurations>(
            configuration.GetSection(HangfireConfigurations.SettingKey)
        );

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IWaitlistPromoter, WaitlistPromoter>();
        services.AddScoped<IWaitlistExpirer, WaitlistExpirer>();

        return services;
    }
}
