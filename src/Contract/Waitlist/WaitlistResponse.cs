using Contract.CustomerPackage;

namespace Contract.Waitlist;

public record WaitlistResponse(
    Guid Id,
    Guid TimetableScheduleId,
    Guid CustomerId,
    Guid CustomerPackageId,
    DateTime JoinedAt
);
