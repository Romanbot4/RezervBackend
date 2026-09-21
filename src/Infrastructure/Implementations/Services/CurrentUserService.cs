using Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Implementations.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? CustomerId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirst("sub");

            return Guid.TryParse(value?.Value, out var customerId) ? customerId : null;
        }
    }
}
