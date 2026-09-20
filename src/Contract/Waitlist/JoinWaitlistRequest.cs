namespace Contract.Waitlist;

public record JoinWaitlistRequest(Guid ScheduleId, Guid CustomerPackageId);
