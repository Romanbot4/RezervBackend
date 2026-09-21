using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class BusinessSeeder : IEntityTypeConfiguration<BusinessEntity>
{
    public void Configure(EntityTypeBuilder<BusinessEntity> builder)
    {
        ICollection<BusinessEntity> businesses = GetBusinesses();
        builder.HasData(businesses);
    }

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
