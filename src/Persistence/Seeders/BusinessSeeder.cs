using Domain.Entities;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public static class BusinessSeeder
{

    public static ICollection<BusinessEntity> GetBusinesses()
    {
        return
        [
            new BusinessEntity(RezerveFitnessId, "Rezerv Fitness")
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
            new BusinessEntity(RhinoYogaId, "Rhino Yoga Studio")
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            },
        ];
    }

    public static readonly Guid RezerveFitnessId = DeterministicGuid.From("business_rezerv_gym");
    public static readonly Guid RhinoYogaId = DeterministicGuid.From("business_rhino_yoga");
}
