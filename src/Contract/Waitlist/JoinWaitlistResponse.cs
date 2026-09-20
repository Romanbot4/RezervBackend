using Contract.CustomerPackage;

namespace Contract.Waitlist
{
    public record JoinWaitlistResponse(
        WaitlistResponse Waitlist,
        CustomerPackageResponse CustomerPackage
    );
}
