using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            BaseConfiguration<Customer>.Configure(builder);
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.FirstName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.LastName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.TradingCode).IsUnicode(true).HasMaxLength(11).IsRequired(false);
            builder.Property(c => c.NationalCode).IsUnicode(false).HasMaxLength(11).IsRequired(false);
            builder.Property(c => c.MobileNumber).IsUnicode(false).HasMaxLength(11);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasIndex(c => new { c.FundId, c.BackOfficeId }).IsUnique();
            builder.HasIndex(c => new { c.FundId, c.TradingCode });
            builder.HasIndex(c => new { c.FundId, c.NationalCode });
            
            builder.HasMany(e => e.Orders)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
