namespace Application.Abstractions.Services;

public interface ICurrentUserService
{
    public Guid? CustomerId { get; }
}
