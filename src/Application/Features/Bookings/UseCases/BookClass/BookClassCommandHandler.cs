using Application.Abstractions.Database;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Features.Bookings.Mappers;
using Application.Features.CustomerPackage.Mappers;
using Application.Features.WaitList.Mappers;
using Contract.Bookings;
using Core.Primitives.Result;
using Domain.Errors;

namespace Application.Features.Bookings.UseCases.BookClass;

public class BookClassCommandHandler(
    IBookingService booking,
    IDistributedLockService locks,
    IUnitOfWork unitOfWork
) : ICommandHandler<BookClassCommand, BookClassResponse>
{
    public async Task<Result<BookClassResponse>> Handle(
        BookClassCommand request,
        CancellationToken cancellationToken
    )
    {
        var context = await booking.ResolveAsync(
            request.ScheduleId,
            request.CustomerPackageId,
            cancellationToken
        );

        await using var scheduleLock =
            await locks.AcquireAsync(
                LockKeys.Schedule(context.Schedule.Id),
                LockKeys.Ttl,
                LockKeys.Wait,
                cancellationToken
            ) ?? throw BookingErrors.ScheduleBusy();

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        if (!context.Schedule.HasAvailableSlot())
        {
            if (!request.JoinWaitlistIfFull)
            {
                throw BookingErrors.ScheduleFull();
            }

            var entry = await booking.JoinWaitlistAsync(context, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Result<BookClassResponse>.Success(
                new BookClassResponse(
                    Outcome: BookingOutcome.Waitlisted,
                    Booking: null,
                    Waitlist: entry.ToWaitlistResponse(),
                    CustomerPackage: context.CustomerPackage.ToCustomerPackageResponse()
                )
            );
        }

        await booking.EnsureNoOverlappingBookingAsync(context, cancellationToken);

        var created = await booking.BookAsync(context, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result<BookClassResponse>.Success(
            new BookClassResponse(
                Outcome: BookingOutcome.Booked,
                Booking: created.ToBookingResponse(),
                Waitlist: null,
                CustomerPackage: context.CustomerPackage.ToCustomerPackageResponse()
            )
        );
    }
}
