using Application.Abstractions.Database;
using Application.Abstractions.DateTime;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Core.Exception.NetworkException;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BookingService(
    ITimetableScheduleRepository schedules,
    ICustomerPackageRepository customerPackages,
    IBookingRepository bookings,
    IWaitlistRepository waitlist,
    ICreditTransactionRepository creditTransactions,
    ICurrentUserService currentUser,
    IDateTime dateTime,
    IUnitOfWork unitOfWork
) : IBookingService
{
    private const int CreditsPerSchedule = 1;

    public async Task<BookingContext> ResolveAsync(
        Guid scheduleId,
        Guid customerPackageId,
        CancellationToken cancellationToken = default
    )
    {
        var customerId = currentUser.CustomerId ?? throw new AuthRequiredException();

        var now = dateTime.UtcNow;

        var schedule =
            await schedules.GetByIdAsync(
                scheduleId,
                alterQuery: query => query.AsNoTracking(),
                cancellationToken: cancellationToken
            ) ?? throw new NotFoundException("Schedule Not Found");

        if (schedule.HasStarted(now))
        {
            throw BookingErrors.ScheduleAlreadyStarted();
        }

        var customerPackage =
            await customerPackages.GetByIdAsync(
                customerPackageId,
                alterQuery: query => query.AsNoTracking(),
                cancellationToken: cancellationToken
            ) ?? throw new NotFoundException("Customer Package Not Found");

        // customer package must belongs to current customer
        if (!customerPackage.BelongsToCustomer(customerId))
        {
            throw BookingErrors.PackageNotBelongToYou();
        }

        // check eligibility and ownerships throws error if not
        EnsurePackageIsUsableFor(customerPackage, schedule, now);

        // check if user already book this schedule
        if (await bookings.HasActiveBookingAsync(customerId, schedule.Id, cancellationToken))
        {
            throw BookingErrors.AlreadyBooked();
        }

        return new BookingContext(customerId, schedule, customerPackage, now);
    }

    public async Task EnsureNoOverlappingBookingAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    )
    {
        // check if user's booking timeline overlaps with existing bookings
        if (
            await bookings.HasOverlappingBookingAsync(
                context.CustomerId,
                context.Schedule.StartTime,
                context.Schedule.EndTime,
                cancellationToken
            )
        )
        {
            throw BookingErrors.OverlappingBooking();
        }
    }

    public async Task<BookingEntity> BookAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    )
    {
        var (_, schedule, customerPackage, now) = context;

        var booking = new BookingEntity(
            id: Guid.NewGuid(),
            customerId: customerPackage.CustomerId,
            timetableScheduleId: schedule.Id,
            customerPackageId: customerPackage.Id,
            bookedAt: now
        );

        if (!await schedules.TryReserveSlotAsync(schedule.Id, cancellationToken))
        {
            throw BookingErrors.ScheduleFull();
        }

        if (!await customerPackages.TryConsumeCreditAsync(customerPackage.Id, cancellationToken))
        {
            throw BookingErrors.InsufficientCredits();
        }

        await bookings.InsertAsync(booking, cancellationToken);

        schedule.ReserveSlot();

        customerPackage.ConsumeCredits(CreditsPerSchedule, now);

        var creditTransaction = new CreditTransactionEntity(
            id: Guid.NewGuid(),
            customerPackageId: customerPackage.Id,
            type: CreditTransactionType.Deduct,
            amount: CreditsPerSchedule,
            reason: "Booking confirmed",
            occurredAt: now
        );

        await creditTransactions.InsertAsync(creditTransaction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return booking;
    }

    public async Task<BookingEntity> PromoteFromWaitlistAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    )
    {
        var (_, schedule, customerPackage, now) = context;

        var booking = new BookingEntity(
            id: Guid.NewGuid(),
            customerId: customerPackage.CustomerId,
            timetableScheduleId: schedule.Id,
            customerPackageId: customerPackage.Id,
            bookedAt: now
        );

        if (!await schedules.TryReserveSlotAsync(schedule.Id, cancellationToken))
        {
            throw BookingErrors.ScheduleFull();
        }

        if (
            !await customerPackages.TryConsumeReservedCreditAsync(
                customerPackage.Id,
                cancellationToken
            )
        )
        {
            throw BookingErrors.InsufficientCredits();
        }

        await bookings.InsertAsync(booking, cancellationToken);

        schedule.ReserveSlot();

        customerPackage.ConsumeReserveCredits(CreditsPerSchedule, now);

        var creditTransaction = new CreditTransactionEntity(
            id: Guid.NewGuid(),
            customerPackageId: customerPackage.Id,
            type: CreditTransactionType.Deduct,
            amount: CreditsPerSchedule,
            reason: "Booking promoted from waitlist. Confirmed.",
            occurredAt: now
        );

        await creditTransactions.InsertAsync(creditTransaction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return booking;
    }

    public async Task DropWaitlistAsync(
        WaitlistEntryEntity entry,
        string reason,
        DateTime now,
        CancellationToken cancellationToken
    )
    {
        await customerPackages.ReleaseReservationAsync(entry.CustomerPackageId, cancellationToken);

        entry.Expire();

        var creditTransaction = new CreditTransactionEntity(
            id: Guid.NewGuid(),
            customerPackageId: entry.CustomerPackageId,
            type: CreditTransactionType.ReleaseReservation,
            amount: CreditsPerSchedule,
            reason: reason,
            occurredAt: now
        )
        {
            WaitlistEntryId = entry.Id,
        };

        await creditTransactions.InsertAsync(creditTransaction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<WaitlistEntryEntity> JoinWaitlistAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    )
    {
        var (customerId, schedule, customerPackage, now) = context;

        // check if user already waiting in waitlist with this schedule
        if (await waitlist.IsWaitingAsync(customerId, schedule.Id, cancellationToken))
        {
            throw BookingErrors.AlreadyWaiting();
        }

        if (!await customerPackages.TryReserveCreditAsync(customerPackage.Id, cancellationToken))
        {
            throw BookingErrors.InsufficientCredits();
        }

        customerPackage.ReservesCredits(CreditsPerSchedule, now);

        var entry = new WaitlistEntryEntity(
            id: Guid.NewGuid(),
            timetableScheduleId: schedule.Id,
            customerId: customerId,
            customerPackageId: customerPackage.Id,
            joinedAt: now
        );

        await waitlist.InsertAsync(entry, cancellationToken);

        var creditTransaction = new CreditTransactionEntity(
            id: Guid.NewGuid(),
            customerPackageId: customerPackage.Id,
            type: CreditTransactionType.Reserve,
            amount: CreditsPerSchedule,
            reason: "Reserved for waitlist",
            occurredAt: now
        );

        await creditTransactions.InsertAsync(creditTransaction, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entry;
    }

    private static void EnsurePackageIsUsableFor(
        CustomerPackageEntity customerPackage,
        TimetableScheduleEntity schedule,
        DateTime now
    )
    {
        if (!customerPackage.BelongsToBusiness(schedule.BusinessId))
        {
            throw BookingErrors.BusinessMismatch();
        }

        if (customerPackage.IsExpired(now))
        {
            throw BookingErrors.PackageExpired(customerPackage.ExpiresAt);
        }

        if (!customerPackage.HasEnoughCredit())
        {
            throw BookingErrors.InsufficientCredits();
        }
    }
}
