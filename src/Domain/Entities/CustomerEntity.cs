using Core.Primitives.Entity;

namespace Domain.Entities;

public class CustomerEntity(Guid id, string name, string email, string passwordHash)
    : AggregateRoot(id),
        IHasTimestamps
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PasswordHash { get; set; } = passwordHash;
    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //Join Entities
    public ICollection<CustomerPackageEntity> CustomerPackages { get; set; } = [];
    public ICollection<BookingEntity> Bookings { get; set; } = [];
}
