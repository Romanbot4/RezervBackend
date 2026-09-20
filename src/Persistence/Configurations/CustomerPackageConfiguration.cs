using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CustomerPackageConfiguration : IEntityTypeConfiguration<CustomerPackageEntity>
{
    public void Configure(EntityTypeBuilder<CustomerPackageEntity> builder)
    {
        builder.ToTable("customer_packages");
        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.ReservedCredits).HasDefaultValue(0);

        builder
            .HasOne(cp => cp.Customer)
            .WithMany(c => c.CustomerPackages)
            .HasForeignKey(cp => cp.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(cp => cp.Package)
            .WithMany()
            .HasForeignKey(cp => cp.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(cp => cp.Business)
            .WithMany()
            .HasForeignKey(cp => cp.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
