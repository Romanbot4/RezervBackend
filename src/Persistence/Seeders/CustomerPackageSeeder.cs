using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class CustomerPackageSeeder : IEntityTypeConfiguration<CustomerPackageEntity>
{
    public static readonly Guid KyawFitnessStandardId = IdFor("kyaw_fitness_standard");
    public static readonly Guid KyawFitnessExpiredId = IdFor("kyaw_fitness_expired");
    public static readonly Guid KyawFitnessExhaustedId = IdFor("kyaw_fitness_exhausted");
    public static readonly Guid ZawFitnessStandardId = IdFor("zaw_fitness_standard");
    public static readonly Guid YanFitnessStandardId = IdFor("yan_fitness_standard");
    public static readonly Guid ZhengFitnessStandardId = IdFor("zheng_fitness_standard");
    public static readonly Guid EaintFitnessStarterId = IdFor("eaint_fitness_starter");
    public static readonly Guid BruceFitnessStarterId = IdFor("bruce_fitness_starter");
    public static readonly Guid KimFitnessStandardId = IdFor("kim_fitness_standard");
    public static readonly Guid DonaldFitnessStandardId = IdFor("donald_fitness_standard");
    public static readonly Guid SteveYogaStandardId = IdFor("steve_yoga_standard");
    public static readonly Guid TonyYogaStandardId = IdFor("tony_yoga_standard");
    public static readonly Guid ThorYogaStandardId = IdFor("thor_yoga_standard");

    public void Configure(EntityTypeBuilder<CustomerPackageEntity> builder)
    {
        builder.HasData(GetCustomerPackages());
    }

    public static ICollection<CustomerPackageEntity> GetCustomerPackages()
    {
        var fitness = BusinessSeeder.RezerveFitnessId;
        var yoga = BusinessSeeder.RhinoYogaId;

        var fitnessStarter = PackageSeeder.FitnessStarterId;
        var fitnessStandard = PackageSeeder.FitnessStandardId;
        var yogaStandard = PackageSeeder.YogaStandardId;

        var now = SeedClock.Now;

        return
        [
            Package(
                KyawFitnessStandardId,
                CustomerSeeder.KyawPyaePhyo,
                fitnessStandard,
                fitness,
                totalCredits: 10,
                remainingCredits: 8,
                purchasedAt: now.AddDays(-2),
                expiresAt: now.AddDays(58)
            ),
            Package(
                KyawFitnessExpiredId,
                CustomerSeeder.KyawPyaePhyo,
                fitnessStarter,
                fitness,
                totalCredits: 5,
                remainingCredits: 5,
                purchasedAt: now.AddDays(-35),
                expiresAt: now.AddDays(-5)
            ),
            Package(
                KyawFitnessExhaustedId,
                CustomerSeeder.KyawPyaePhyo,
                fitnessStarter,
                fitness,
                totalCredits: 5,
                remainingCredits: 0,
                purchasedAt: now.AddDays(-10),
                expiresAt: now.AddDays(20)
            ),
            Package(
                ZawFitnessStandardId,
                CustomerSeeder.ZawMyoTun,
                fitnessStandard,
                fitness,
                totalCredits: 10,
                remainingCredits: 8,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57)
            ),
            Package(
                YanFitnessStandardId,
                CustomerSeeder.YanNaingKyaw,
                fitnessStandard,
                fitness,
                totalCredits: 10,
                remainingCredits: 8,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57)
            ),
            Package(
                ZhengFitnessStandardId,
                CustomerSeeder.ZhengYu,
                fitnessStandard,
                fitness,
                totalCredits: 10,
                remainingCredits: 8,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57)
            ),
            Package(
                EaintFitnessStarterId,
                CustomerSeeder.EaintPan,
                fitnessStarter,
                fitness,
                totalCredits: 5,
                remainingCredits: 4,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(27)
            ),
            Package(
                BruceFitnessStarterId,
                CustomerSeeder.BruceWill,
                fitnessStarter,
                fitness,
                totalCredits: 5,
                remainingCredits: 4,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(27)
            ),
            Package(
                KimFitnessStandardId,
                CustomerSeeder.KimJongUn,
                fitnessStandard,
                fitness,
                totalCredits: 10,
                remainingCredits: 10,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57),
                reservedCredits: 1
            ),
            Package(
                DonaldFitnessStandardId,
                CustomerSeeder.DonaldTrump,
                fitnessStandard,
                fitness,
                totalCredits: 10,
                remainingCredits: 10,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57),
                reservedCredits: 1
            ),
            Package(
                SteveYogaStandardId,
                CustomerSeeder.SteveRoger,
                yogaStandard,
                yoga,
                totalCredits: 10,
                remainingCredits: 9,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57)
            ),
            Package(
                TonyYogaStandardId,
                CustomerSeeder.TonyStark,
                yogaStandard,
                yoga,
                totalCredits: 10,
                remainingCredits: 9,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57)
            ),
            Package(
                ThorYogaStandardId,
                CustomerSeeder.ThorOdinson,
                yogaStandard,
                yoga,
                totalCredits: 10,
                remainingCredits: 9,
                purchasedAt: now.AddDays(-3),
                expiresAt: now.AddDays(57)
            ),
        ];
    }

    private static CustomerPackageEntity Package(
        Guid id,
        string customerName,
        Guid packageId,
        Guid businessId,
        int totalCredits,
        int remainingCredits,
        DateTime purchasedAt,
        DateTime expiresAt,
        int reservedCredits = 0
    ) =>
        new(
            id: id,
            customerId: CustomerSeeder.IdFor(customerName),
            packageId: packageId,
            businessId: businessId,
            totalCredits: totalCredits,
            remainingCredits: remainingCredits,
            reservedCredits: reservedCredits,
            purchasedAt: purchasedAt,
            expiresAt: expiresAt
        )
        {
            AddedAt = SeedClock.Now,
            UpdatedAt = SeedClock.Now,
        };

    private static Guid IdFor(string key) => DeterministicGuid.From($"customer_package_{key}");
}
