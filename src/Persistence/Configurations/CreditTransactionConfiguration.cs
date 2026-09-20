using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CreditTransactionConfiguration : IEntityTypeConfiguration<CreditTransactionEntity>
{
    public void Configure(EntityTypeBuilder<CreditTransactionEntity> builder)
    {
        builder.ToTable("credit_transactions");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Reason).HasMaxLength(300).IsRequired();

        builder
            .HasOne(t => t.CustomerPackage)
            .WithMany()
            .HasForeignKey(t => t.CustomerPackageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
