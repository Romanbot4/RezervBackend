namespace Application.Abstractions.Services;

/// <summary>
/// If a class ends while a customer is still waiting, the credit reservation is released
/// </summary>
public interface IWaitlistExpirer
{
    Task<int> ReleaseEndedAsync(CancellationToken cancellationToken = default);
}
