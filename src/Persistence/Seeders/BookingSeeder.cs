using Domain.Entities;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public static class BookingSeeder
{

    public static ICollection<BookingEntity> GetBookings()
    {
        var bookedAt = SeedClock.Now.AddHours(-6);

        return
        [
            // Strength Foundations, 5 of 5. Full, with a waitlist behind it.
            Booking(
                CustomerSeeder.ZawMyoTun,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.ZawFitnessStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.YanNaingKyaw,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.YanFitnessStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.ZhengYu,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.ZhengFitnessStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.EaintPan,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.EaintFitnessStarterId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.BruceWill,
                TimetableScheduleSeeder.StrengthFoundationsId,
                CustomerPackageSeeder.BruceFitnessStarterId,
                bookedAt
            ),
            // Morning Upper Body, 3 of 15. A different day to Strength Foundations, so these three
            // customers hold both without overlapping.
            Booking(
                CustomerSeeder.ZawMyoTun,
                TimetableScheduleSeeder.MorningUpperId,
                CustomerPackageSeeder.ZawFitnessStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.YanNaingKyaw,
                TimetableScheduleSeeder.MorningUpperId,
                CustomerPackageSeeder.YanFitnessStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.ZhengYu,
                TimetableScheduleSeeder.MorningUpperId,
                CustomerPackageSeeder.ZhengFitnessStandardId,
                bookedAt
            ),
            // Starts in two hours, so cancelling this one is inside the window and earns no refund.
            Booking(
                CustomerSeeder.KyawPyaePhyo,
                TimetableScheduleSeeder.LunchtimePilatesId,
                CustomerPackageSeeder.KyawFitnessStandardId,
                bookedAt
            ),
            // Three days out, so cancelling this one does earn the credit back.
            Booking(
                CustomerSeeder.KyawPyaePhyo,
                TimetableScheduleSeeder.EarlyBirdCardioId,
                CustomerPackageSeeder.KyawFitnessStandardId,
                bookedAt
            ),
            // Zendaya Yoga, 3 of 20, on the other business.
            Booking(
                CustomerSeeder.SteveRoger,
                TimetableScheduleSeeder.ZendayaYogaId,
                CustomerPackageSeeder.SteveYogaStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.TonyStark,
                TimetableScheduleSeeder.ZendayaYogaId,
                CustomerPackageSeeder.TonyYogaStandardId,
                bookedAt
            ),
            Booking(
                CustomerSeeder.ThorOdinson,
                TimetableScheduleSeeder.ZendayaYogaId,
                CustomerPackageSeeder.ThorYogaStandardId,
                bookedAt
            ),
        ];
    }

    private static BookingEntity Booking(
        string customerName,
        Guid scheduleId,
        Guid customerPackageId,
        DateTime bookedAt
    )
    {
        var customerId = CustomerSeeder.IdFor(customerName);

        return new BookingEntity(
            id: DeterministicGuid.From($"booking_{customerId}_{scheduleId}"),
            customerId: customerId,
            timetableScheduleId: scheduleId,
            customerPackageId: customerPackageId,
            bookedAt: bookedAt
        )
        {
            AddedAt = SeedClock.Now,
            UpdatedAt = SeedClock.Now,
        };
    }
}
