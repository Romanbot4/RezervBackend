using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class PackageSeeder : IEntityTypeConfiguration<PackageEntity>
{
    public static readonly Guid FitnessStarterId = DeterministicGuid.From(
        "package_fitness_starter"
    );
    public static readonly Guid FitnessStandardId = DeterministicGuid.From(
        "package_fitness_standard"
    );
    public static readonly Guid FitnessProId = DeterministicGuid.From("package_fitness_pro");
    public static readonly Guid YogaStarterId = DeterministicGuid.From("package_yoga_starter");
    public static readonly Guid YogaStandardId = DeterministicGuid.From("package_yoga_standard");
    public static readonly Guid YogaProId = DeterministicGuid.From("package_yoga_pro");

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
                FitnessStarterId,
                rezerveFitnessId,
                "Fitness Starter",
                credits: 5,
                validityDays: 30,
                price: 49.00m
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
            new PackageEntity(
                FitnessStandardId,
                rezerveFitnessId,
                "Fitness Standard",
                credits: 10,
                validityDays: 60,
                price: 89.00m
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
            new PackageEntity(
                FitnessProId,
                rezerveFitnessId,
                "Fitness Pro",
                credits: 20,
                validityDays: 90,
                price: 159.00m
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
            new PackageEntity(
                YogaStarterId,
                rhinoYogaId,
                "Yoga Starter",
                credits: 5,
                validityDays: 30,
                price: 45.00m
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
            new PackageEntity(
                YogaStandardId,
                rhinoYogaId,
                "Yoga Standard",
                credits: 10,
                validityDays: 60,
                price: 85.00m
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
            new PackageEntity(
                YogaProId,
                rhinoYogaId,
                "Yoga Pro",
                credits: 20,
                validityDays: 90,
                price: 149.00m
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
        ];
    }
}
