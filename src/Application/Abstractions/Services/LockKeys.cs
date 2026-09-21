namespace Application.Abstractions.Services;

using System;

public static class LockKeys
{
    public static readonly TimeSpan Ttl = TimeSpan.FromSeconds(5);

    public static readonly TimeSpan Wait = TimeSpan.FromSeconds(3);

    public static string Schedule(Guid scheduleId) => $"lock:schedule:{scheduleId}";
}
