using Application.Abstractions.DateTime;
using Application.Abstractions.Services;
using Infrastructure.Common.ExceptionHandlers;
using Infrastructure.Implementations.DateTime;
using Infrastructure.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTime, MachineDateTime>();
        services.AddScoped<IHashPasswordService, Md5HashPasswordService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
