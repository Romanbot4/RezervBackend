using Contract.CustomerPackage;
using Contract.Waitlist;

namespace Contract.Bookings;

public enum BookingOutcome
{
    Booked = 1,
    Waitlisted = 2,
    Cancelled = 3,
}

public record BookClassResponse(
    BookingOutcome Outcome,
    BookingResponse? Booking,
    WaitlistResponse? Waitlist,
    CustomerPackageResponse CustomerPackage
);
