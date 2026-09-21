using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class WaitlistEntrySeeder : IEntityTypeConfiguration<WaitlistEntryEntity>
{
    public void Configure(EntityTypeBuilder<WaitlistEntryEntity> builder)
    {
        builder.HasData(GetWaitlistEntries());
    }

    public static ICollection<WaitlistEntryEntity> GetWaitlistEntries()
    {
        return
        [
            Entry(
                CustomerSeeder.KimJongUn,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.KimFitnessStandardId,
                joinedAt: SeedClock.Now.AddHours(-2)
            ),
            Entry(
                CustomerSeeder.DonaldTrump,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.DonaldFitnessStandardId,
                joinedAt: SeedClock.Now.AddHours(-1)
            ),
        ];
    }

    private static WaitlistEntryEntity Entry(
        string customerName,
        Guid scheduleId,
        Guid customerPackageId,
        DateTime joinedAt
    )
    {
        var customerId = CustomerSeeder.IdFor(customerName);

        return new WaitlistEntryEntity(
            id: DeterministicGuid.From($"waitlist_{customerId}_{scheduleId}"),
            timetableScheduleId: scheduleId,
            customerId: customerId,
            customerPackageId: customerPackageId,
            joinedAt: joinedAt
        )
        {
            AddedAt = SeedClock.Now,
            UpdatedAt = SeedClock.Now,
        };
    }
}
