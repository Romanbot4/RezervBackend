using Application.Features.Bookings.UseCases.BookClass;
using Application.Features.Bookings.UseCases.CancelBooking;
using Application.Features.Bookings.UseCases.GetMyBookings;
using Contract.Bookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[Route("api/bookings")]
[Authorize]
public class BookingController(ISender sender) : ApiController(sender)
{
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyBookings(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetMyBookingsQuery(), cancellationToken);

        return Respond(result);
    }

    [HttpPost]
    public async Task<IActionResult> BookClass(
        [FromBody] BookClassRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(
            new BookClassCommand(
                request.ScheduleId,
                request.CustomerPackageId,
                request.JoinWaitlistIfFull
            ),
            cancellationToken
        );

        return Respond(result, StatusCodes.Status201Created);
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelBooking(
        [FromBody] CancelBookingRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Sender.Send(
            new CancelBookingCommand(request.BookingId),
            cancellationToken
        );

        return Respond(result);
    }
}
