using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class PackageSeeder : IEntityTypeConfiguration<PackageEntity>
{
    public void Configure(EntityTypeBuilder<PackageEntity> builder)
    {
        ICollection<PackageEntity> packages = GetPackages();

        builder.HasData(packages);
    }

    public static ICollection<PackageEntity> GetPackages()
    {
        var rezerveFitnessId = BusinessSeeder.RezerveFitnessId;
        var rhinoYogaId = BusinessSeeder.RhinoYogaId;

        return
        [
            new PackageEntity(
                DeterministicGuid.From("package_fitness_starter"),
                rezerveFitnessId,
                "Fitness Starter",
                credits: 5,
                validityDays: 30,
                price: 49.00m
            ),
            new PackageEntity(
                DeterministicGuid.From("package_fitness_standard"),
                rezerveFitnessId,
                "Fitness Standard",
                credits: 10,
                validityDays: 60,
                price: 89.00m
            ),
            new PackageEntity(
                DeterministicGuid.From("package_fitness_pro"),
                rezerveFitnessId,
                "Fitness Pro",
                credits: 20,
                validityDays: 90,
                price: 159.00m
            ),
            new PackageEntity(
                DeterministicGuid.From("package_yoga_starter"),
                rhinoYogaId,
                "Yoga Starter",
                credits: 5,
                validityDays: 30,
                price: 45.00m
            ),
            new PackageEntity(
                DeterministicGuid.From("package_yoga_standard"),
                rhinoYogaId,
                "Yoga Standard",
                credits: 10,
                validityDays: 60,
                price: 85.00m
            ),
            new PackageEntity(
                DeterministicGuid.From("package_yoga_pro"),
                rhinoYogaId,
                "Yoga Pro",
                credits: 20,
                validityDays: 90,
                price: 149.00m
            ),
        ];
    }
}
