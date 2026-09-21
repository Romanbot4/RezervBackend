using Domain.Entities;

namespace Application.Abstractions.Services;

using System;

public record BookingContext(
    Guid CustomerId,
    TimetableScheduleEntity Schedule,
    CustomerPackageEntity CustomerPackage,
    DateTime Now
);

/// <summary>
/// The booking rules shared between BookClassCommandHandle and JoinWaitlistCommandHandler.
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Loads the schedule and package for the signed-in customer.
    /// Check every business rules and ownership.
    /// Throws imediately when even one rule fail.
    /// </summary>
    Task<BookingContext> ResolveAsync(
        Guid scheduleId,
        Guid customerPackageId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Rejects booking something the customer has already booked.
    /// </summary>
    Task EnsureNoOverlappingBookingAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    );

    /// <summary>Takes the slot and spends one credit.</summary>
    Task<BookingEntity> BookAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Joins the list and hold one credit without spending it. The credit is only deducted if
    /// the customer is promoted to opening slot.
    /// </summary>
    Task<WaitlistEntryEntity> JoinWaitlistAsync(
        BookingContext context,
        CancellationToken cancellationToken = default
    );
    Task<BookingEntity> PromoteFromWaitlistAsync(
        BookingContext bookingContext,
        CancellationToken cancellationToken
    );

    Task DropWaitlistAsync(
        WaitlistEntryEntity entry,
        string reason,
        DateTime now,
        CancellationToken cancellationToken
    );
}
