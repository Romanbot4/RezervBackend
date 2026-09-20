using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.RefundApplied).HasDefaultValue(false);

        builder
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(b => b.TimetableSchedule)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.TimetableScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(b => b.CustomerPackage)
            .WithMany()
            .HasForeignKey(b => b.CustomerPackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
