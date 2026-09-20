using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class TimetableScheduleSeeder : IEntityTypeConfiguration<TimetableScheduleEntity>
{
    public void Configure(EntityTypeBuilder<TimetableScheduleEntity> builder)
    {
        ICollection<TimetableScheduleEntity> packages = GetTimetableSchedules();

        builder.HasData(packages);
    }

    public static ICollection<TimetableScheduleEntity> GetTimetableSchedules()
    {
        var rezerveFitnessId = BusinessSeeder.RezerveFitnessId;
        var rhinoYogaId = BusinessSeeder.RhinoYogaId;

        var now = DateTime.UtcNow;
        var todayMidnight = GetTodaySingaporeMidnightInUtc();

        return
        [
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_morning_upper"),
                businessId: rezerveFitnessId,
                className: "Morning Upper Body Workout",
                instructorName: "Brad Pitt",
                startTime: todayMidnight.AddDays(1).AddHours(9),
                endTime: todayMidnight.AddDays(1).AddHours(10),
                availableSlots: 15
            ),
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_morning_cardio"),
                businessId: rezerveFitnessId,
                className: "Morning Cardio Class",
                instructorName: "Justin Bieber",
                startTime: todayMidnight.AddDays(1).AddHours(9).AddMinutes(30),
                endTime: todayMidnight.AddDays(1).AddHours(10).AddMinutes(30),
                availableSlots: 12
            ),
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_before_lunchtime_upper"),
                businessId: rezerveFitnessId,
                className: "Lunchtime Upper Body Workout",
                instructorName: "Taylor Swift",
                startTime: todayMidnight.AddDays(1).AddHours(10),
                endTime: todayMidnight.AddDays(1).AddHours(11),
                availableSlots: 10
            ),
            // For low capacity
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_strength_foundations"),
                businessId: rezerveFitnessId,
                className: "Strength Foundations",
                instructorName: "Bradd Pitt",
                startTime: todayMidnight.AddDays(2).AddHours(18),
                endTime: todayMidnight.AddDays(2).AddHours(19),
                availableSlots: 5
            ),
            // Starts in two hours. To test no refund.
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_lunchtime_pilates"),
                businessId: rezerveFitnessId,
                className: "Lunchtime Pilates",
                instructorName: "Sofia Rossi",
                startTime: now.AddHours(2),
                endTime: now.AddHours(3),
                availableSlots: 10
            ),
            // Starts in 3 days and more. To test refund.
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_early_bird_cardio"),
                businessId: rezerveFitnessId,
                className: "Early Bird Cardio",
                instructorName: "Tom Holland",
                startTime: todayMidnight.AddDays(3).AddHours(7),
                endTime: todayMidnight.AddDays(3).AddHours(8),
                availableSlots: 8
            ),
            new TimetableScheduleEntity(
                id: DeterministicGuid.From("time_table_zendaya_yoga"),
                businessId: rhinoYogaId,
                className: "Zendaya Yoga",
                instructorName: "Zendaya",
                startTime: todayMidnight.AddDays(1).AddHours(8),
                endTime: todayMidnight.AddDays(1).AddHours(9).AddMinutes(15),
                availableSlots: 20
            ),
        ];
    }

    private static DateTime GetTodaySingaporeMidnightInUtc()
    {
        TimeZoneInfo sgTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Singapore");

        DateTime currentSgTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgTimeZone);

        return currentSgTime.Date;
    }
}
