using Application.Abstractions.DateTime;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Database;
using Application.Features.Bookings.Mappers;
using Application.Features.CustomerPackage.Mappers;
using Contract.Bookings;
using Core.Exception.NetworkException;
using Core.Primitives.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;

namespace Application.Features.Bookings.UseCases.CancelBooking;

/// <summary>
/// Cancellation
/// Users may cancel bookings.
///
/// Rules
/// 1. If cancelled more than 4 hours before class start time:
///    ○ 1 credit should be refunded
/// 2. If cancelled within 4 hours:
///    ○ no refund
/// </summary>
public class CancelBookingCommandHandler(
    IBookingRepository bookings,
    ITimetableScheduleRepository schedules,
    ICustomerPackageRepository customerPackages,
    ICreditTransactionRepository creditTransactions,
    ICurrentUserService currentUser,
    IWaitlistPromoter waitlistPromoter,
    IDateTime dateTime,
    IUnitOfWork unitOfWork
) : ICommandHandler<CancelBookingCommand, BookClassResponse>
{
    public async Task<Result<BookClassResponse>> Handle(
        CancelBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var customerId = currentUser.CustomerId ?? throw new AuthRequiredException();

        var now = dateTime.UtcNow;

        var booking =
            await bookings.GetByIdAsync(request.BookingId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Booking Not Found");

        if (booking.CustomerId != customerId)
        {
            throw BookingErrors.BookingNotBelongToYou();
        }

        if (booking.Status != BookingStatus.Booked)
        {
            throw BookingErrors.BookingNotActive();
        }

        var schedule =
            await schedules.GetByIdAsync(
                booking.TimetableScheduleId,
                cancellationToken: cancellationToken
            ) ?? throw new NotFoundException("Schedule Not Found");

        var result = await CancelBooking(booking, schedule, now, cancellationToken);

        return Result<BookClassResponse>.Success(result);
    }

    private async Task<BookClassResponse> CancelBooking(
        BookingEntity booking,
        TimetableScheduleEntity schedule,
        DateTime now,
        CancellationToken cancellationToken
    )
    {
        var customerPackage =
            await customerPackages.GetByIdAsync(
                booking.CustomerPackageId,
                cancellationToken: cancellationToken
            ) ?? throw new NotFoundException("Customer package not found");

        // Refund only is 4-hour or before start
        var refundApplied = schedule.QualifiesForRefund(now) && !customerPackage.IsExpired(now);

        booking.Cancel(now, refundApplied);

        schedule.ReleaseSlot();

        if (refundApplied)
        {
            await customerPackages.RefundCreditAsync(booking.CustomerPackageId, cancellationToken);

            var creditTransaction = new CreditTransactionEntity(
                id: Guid.NewGuid(),
                customerPackageId: customerPackage.Id,
                type: CreditTransactionType.Refund,
                amount: 1,
                reason: "Booking cancel refund",
                occurredAt: now
            )
            {
                BookingId = booking.Id,
            };

            await creditTransactions.InsertAsync(creditTransaction, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var promotion = await waitlistPromoter.PromoteNextAsync(schedule, cancellationToken);

        return new BookClassResponse(
            Outcome: BookingOutcome.Cancelled,
            Booking: booking.ToBookingResponse(),
            CustomerPackage: customerPackage.ToCustomerPackageResponse(),
            Waitlist: null
        );
    }
}
