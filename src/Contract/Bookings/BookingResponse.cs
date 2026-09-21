namespace Contract.Bookings;

public record BookingResponse(
    Guid Id,
    Guid CustomerId,
    Guid TimetableScheduleId,
    Guid CustomerPackageId,
    DateTime BookedAt,
    bool RefundApplied
);
