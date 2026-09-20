using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../WebApi");

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(Path.Combine(basePath, "appsettings.json"), optional: false)
            .AddJsonFile(Path.Combine(basePath, $"appsettings.{environment}.json"))
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new ApplicationDbContext(optionBuilder.Options, null!, null!);
    }
}
