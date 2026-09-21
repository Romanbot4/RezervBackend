using Application.Abstractions.Database;
using Application.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Common.Abstractions;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

/// <summary>
/// The sample data used to be attached with HasData. That bakes every value into the migration at
/// the moment it is scaffolded, so the schedules froze on that day and drifted into the past, and
/// EF reported pending model changes on every update because SeedClock.Now moved.
/// Seeding at runtime keeps the relative times honest and leaves the model static.
/// </summary>
public class DatabaseSeeder(
    IDbContext context,
    IUnitOfWork unitOfWork,
    ILogger<DatabaseSeeder> logger
) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await context.Set<BusinessEntity>().AnyAsync(cancellationToken))
        {
            return;
        }

        await context.InsertRangeAsync([.. BusinessSeeder.GetBusinesses()], cancellationToken);
        await context.InsertRangeAsync([.. CustomerSeeder.GetCustomers()], cancellationToken);
        await context.InsertRangeAsync([.. PackageSeeder.GetPackages()], cancellationToken);
        await context.InsertRangeAsync(
            [.. CustomerPackageSeeder.GetCustomerPackages()],
            cancellationToken
        );
        await context.InsertRangeAsync(
            [.. TimetableScheduleSeeder.GetTimetableSchedules()],
            cancellationToken
        );
        await context.InsertRangeAsync([.. BookingSeeder.GetBookings()], cancellationToken);
        await context.InsertRangeAsync(
            [.. WaitlistEntrySeeder.GetWaitlistEntries()],
            cancellationToken
        );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded the sample data relative to {Now}", SeedClock.Now);
    }
}
