namespace Persistence.Common.Abstractions;

public interface ISeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
