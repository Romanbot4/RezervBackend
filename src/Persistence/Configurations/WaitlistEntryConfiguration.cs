using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class WaitlistEntryConfiguration : IEntityTypeConfiguration<WaitlistEntryEntity>
{
    public void Configure(EntityTypeBuilder<WaitlistEntryEntity> builder)
    {
        builder.ToTable("waitlist_entries");
        builder.HasKey(w => w.Id);

        builder
            .HasOne(w => w.Customer)
            .WithMany()
            .HasForeignKey(w => w.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(w => w.TimetableSchedule)
            .WithMany(s => s.WaitlistEntries)
            .HasForeignKey(w => w.TimetableScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(w => w.CustomerPackage)
            .WithMany()
            .HasForeignKey(w => w.CustomerPackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
