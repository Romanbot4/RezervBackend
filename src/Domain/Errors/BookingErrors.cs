using Core.Exception;
using Core.Exception.NetworkException;

namespace Domain.Errors;

public static class BookingErrors
{
    public static CoreException ScheduleAlreadyStarted() =>
        new UnprocessableException("Schedule already started");

    public static CoreException PackageNotBelongToYou() =>
        new ForbiddenException("This customer package do not belongs to you");

    public static CoreException BookingNotBelongToYou() =>
        new ForbiddenException("This booking do not belongs to you");

    public static CoreException BusinessMismatch() =>
        new ForbiddenException("This customer package do not belongs to this business");

    public static CoreException PackageExpired(DateTime expiresAt) =>
        new ForbiddenException($"This customer package is already expired at {expiresAt}");

    public static CoreException InsufficientCredits(string? message = null) =>
        new ForbiddenException(message ?? "You do not have enough credits");

    public static CoreException AlreadyBooked() =>
        new ForbiddenException($"You already booked this schedule");

    public static Exception OverlappingBooking() =>
        new ForbiddenException(
            $"You already booked a schedule which overlap with this scuedule. Cannot proceed."
        );

    public static CoreException AlreadyWaiting() =>
        new ForbiddenException($"You already waiting for this schedule");

    public static CoreException ScheduleFull() =>
        new UnprocessableException(
            "This schedule is fully booked. Join the waitlist to be booked automatically if a slot frees up."
        );

    public static CoreException BookingNotActive() =>
        new UnprocessableException("This booking is not active and cannot be cancelled");
}
