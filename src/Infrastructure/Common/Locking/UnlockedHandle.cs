using Application.Abstractions.Services;

namespace Infrastructure.Common.Locking;

public sealed class UnlockedHandle(string key) : ILockHandle
{
    public string Key => key;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
