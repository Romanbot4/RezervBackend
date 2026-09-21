namespace Application.Abstractions.Services;

using System;

public interface IDistributedLockService
{
    Task<ILockHandle?> AcquireAsync(
        string key,
        TimeSpan ttl,
        TimeSpan wait,
        CancellationToken cancellationToken = default
    );
}
