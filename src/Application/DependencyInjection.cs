using Application.Configurations;
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
        services.Configure<JwtConfigurations>(options =>
            configuration.GetSection(JwtConfigurations.SettingKey)
        );

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}
