using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class TimetableScheduleConfiguration : IEntityTypeConfiguration<TimetableScheduleEntity>
{
    public void Configure(EntityTypeBuilder<TimetableScheduleEntity> builder)
    {
        builder.ToTable("timetable_schedules");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ClassName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.InstructorName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.BookedCount).HasDefaultValue(0);

        builder
            .HasOne(s => s.Business)
            .WithMany(b => b.TimetableSchedules)
            .HasForeignKey(s => s.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        // Serves the timetable listing, which always filters by business and/or day.
        builder.HasIndex(s => new { s.BusinessId, s.StartTime });
        builder.HasIndex(s => s.EndTime);
    }
}
