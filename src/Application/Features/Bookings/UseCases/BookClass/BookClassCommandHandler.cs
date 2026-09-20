using Application.Abstractions.Services;
using Application.Features.Bookings.Mappers;
using Application.Features.CustomerPackage.Mappers;
using Application.Features.WaitList.Mappers;
using Contract.Bookings;
using Core.Primitives.Result;
using Domain.Errors;
using MediatR;

namespace Application.Features.Bookings.UseCases.BookClass;

public class BookClassCommandHandler(IBookingService booking)
    : IRequestHandler<BookClassCommand, Result<BookClassResponse>>
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

        if (!context.Schedule.HasAvailableSlot())
        {
            if (!request.JoinWaitlistIfFull)
            {
                throw BookingErrors.ScheduleFull();
            }

            var entry = await booking.JoinWaitlistAsync(context, cancellationToken);

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
