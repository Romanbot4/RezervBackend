namespace Infrastructure.Common.Jobs;

using Application.Abstractions.Services;
using Hangfire;

/// <summary>
/// The only Hangfire aware surface. Keeps the attributes off the Application layer.
/// The sweep is already idempotent, so no retries. The next tick is the retry.
/// </summary>
[DisableConcurrentExecution(timeoutInSeconds: 60)]
[AutomaticRetry(Attempts = 0)]
public class WaitlistExpiryJob(IWaitlistExpirer expirer)
{
    public const string RecurringJobId = "waitlist-release-ended";

    public Task RunAsync(CancellationToken cancellationToken) =>
        expirer.ReleaseEndedAsync(cancellationToken);
}
