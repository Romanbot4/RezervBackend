namespace Application.Abstractions.Services;

public interface ILockHandle : IAsyncDisposable
{
    string Key { get; }
}
