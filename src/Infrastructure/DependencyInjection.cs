using Application.Abstractions.DateTime;
using Infrastructure.Implementations.DateTime;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTime, MachineDateTime>();

        return services;
    }
}
