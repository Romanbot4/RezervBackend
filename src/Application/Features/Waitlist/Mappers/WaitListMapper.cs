using Contract.Waitlist;
using Domain.Entities;

namespace Application.Features.WaitList.Mappers;

public static class WaitListEntryMapper
{
    public static WaitlistResponse ToWaitlistResponse(this WaitlistEntryEntity waitlistEntry)
    {
        return new WaitlistResponse(
            Id: waitlistEntry.Id,
            TimetableScheduleId: waitlistEntry.TimetableScheduleId,
            CustomerId: waitlistEntry.CustomerId,
            CustomerPackageId: waitlistEntry.CustomerPackageId,
            JoinedAt: waitlistEntry.JoinedAt
        );
    }
}
