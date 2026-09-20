using Application.Database;

namespace Persistence.Common.Abstractions;

public interface ISeeder
{
    Task SeedAsync(IDbContext context);
}
