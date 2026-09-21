using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public class TimetableScheduleSeeder : IEntityTypeConfiguration<TimetableScheduleEntity>
{
    public static readonly Guid MorningUpperId = DeterministicGuid.From("time_table_morning_upper");
    public static readonly Guid MorningCardioId = DeterministicGuid.From(
        "time_table_morning_cardio"
    );
    public static readonly Guid LunchtimeUpperId = DeterministicGuid.From(
        "time_table_before_lunchtime_upper"
    );
    public static readonly Guid StrengthFoundationsId = DeterministicGuid.From(
        "time_table_strength_foundations"
    );
    public static readonly Guid LunchtimePilatesId = DeterministicGuid.From(
        "time_table_lunchtime_pilates"
    );
    public static readonly Guid EarlyBirdCardioId = DeterministicGuid.From(
        "time_table_early_bird_cardio"
    );
    public static readonly Guid ZendayaYogaId = DeterministicGuid.From("time_table_zendaya_yoga");
    public static readonly Guid SunriseBootcampId = DeterministicGuid.From(
        "time_table_sunrise_bootcamp"
    );

    public void Configure(EntityTypeBuilder<TimetableScheduleEntity> builder)
    {
        builder.HasData(GetTimetableSchedules());
    }

    public static ICollection<TimetableScheduleEntity> GetTimetableSchedules()
    {
        var rezerveFitnessId = BusinessSeeder.RezerveFitnessId;
        var rhinoYogaId = BusinessSeeder.RhinoYogaId;

        var now = SeedClock.Now;
        var todayMidnight = GetTodaySingaporeMidnightInUtc();

        return
        [
            Schedule(
                MorningUpperId,
                rezerveFitnessId,
                "Morning Upper Body Workout",
                "Brad Pitt",
                todayMidnight.AddDays(1).AddHours(9),
                todayMidnight.AddDays(1).AddHours(10),
                availableSlots: 15,
                bookedCount: 3
            ),
            // Left empty on purpose, for the mixed availability the brief asks for.
            Schedule(
                MorningCardioId,
                rezerveFitnessId,
                "Morning Cardio Class",
                "Justin Bieber",
                todayMidnight.AddDays(1).AddHours(9).AddMinutes(30),
                todayMidnight.AddDays(1).AddHours(10).AddMinutes(30),
                availableSlots: 12,
                bookedCount: 0
            ),
            Schedule(
                LunchtimeUpperId,
                rezerveFitnessId,
                "Lunchtime Upper Body Workout",
                "Taylor Swift",
                todayMidnight.AddDays(1).AddHours(10),
                todayMidnight.AddDays(1).AddHours(11),
                availableSlots: 10,
                bookedCount: 0
            ),
            // Seeded to capacity, with a waitlist queued behind it. This is the class to cancel
            // from if you want to watch a promotion happen.
            Schedule(
                StrengthFoundationsId,
                rezerveFitnessId,
                "Strength Foundations",
                "Bradd Pitt",
                todayMidnight.AddDays(2).AddHours(18),
                todayMidnight.AddDays(2).AddHours(19),
                availableSlots: 5,
                bookedCount: 5
            ),
            // Starts in two hours, so cancelling falls inside the 4 hour window. No refund.
            Schedule(
                LunchtimePilatesId,
                rezerveFitnessId,
                "Lunchtime Pilates",
                "Sofia Rossi",
                now.AddHours(2),
                now.AddHours(3),
                availableSlots: 10,
                bookedCount: 1
            ),
            // Days away, so cancelling earns the credit back.
            Schedule(
                EarlyBirdCardioId,
                rezerveFitnessId,
                "Early Bird Cardio",
                "Tom Holland",
                todayMidnight.AddDays(3).AddHours(7),
                todayMidnight.AddDays(3).AddHours(8),
                availableSlots: 8,
                bookedCount: 1
            ),
            // Already finished schedule to check failure on booking
            Schedule(
                SunriseBootcampId,
                rezerveFitnessId,
                "Sunrise Bootcamp",
                "Chris Hemsworth",
                now.AddHours(-3),
                now.AddHours(-2),
                availableSlots: 10,
                bookedCount: 0
            ),
            Schedule(
                ZendayaYogaId,
                rhinoYogaId,
                "Zendaya Yoga",
                "Zendaya",
                todayMidnight.AddDays(1).AddHours(8),
                todayMidnight.AddDays(1).AddHours(9).AddMinutes(15),
                availableSlots: 20,
                bookedCount: 3
            ),
        ];
    }

    private static TimetableScheduleEntity Schedule(
        Guid id,
        Guid businessId,
        string className,
        string instructorName,
        DateTime startTime,
        DateTime endTime,
        int availableSlots,
        int bookedCount
    ) =>
        new(
            id: id,
            businessId: businessId,
            className: className,
            instructorName: instructorName,
            startTime: startTime,
            endTime: endTime,
            availableSlots: availableSlots,
            bookedCount: bookedCount
        )
        {
            AddedAt = SeedClock.Now,
            UpdatedAt = SeedClock.Now,
        };

    private static DateTime GetTodaySingaporeMidnightInUtc()
    {
        TimeZoneInfo sgTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Singapore");

        DateTime currentSgTime = TimeZoneInfo.ConvertTimeFromUtc(SeedClock.Now, sgTimeZone);

        // Converted back, otherwise this returns Singapore local midnight while claiming to be UTC
        // and every seeded class lands 8 hours away from where it says it is.
        return TimeZoneInfo.ConvertTimeToUtc(currentSgTime.Date, sgTimeZone);
    }
}
