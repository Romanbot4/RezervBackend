using Contract.Bookings;
using Domain.Entities;

namespace Application.Features.Bookings.Mappers;

public static class BookingMapper
{
    public static BookingResponse ToBookingResponse(this BookingEntity booking)
    {
        return new BookingResponse(
            booking.Id,
            booking.CustomerId,
            booking.TimetableScheduleId,
            booking.CustomerPackageId,
            booking.BookedAt
        );
    }
}
