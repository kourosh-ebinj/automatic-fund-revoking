using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.ThirdParties;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations.ThirdParties
{
    public class RayanCustomerConfiguration : IEntityTypeConfiguration<RayanCustomer>
    {
        public void Configure(EntityTypeBuilder<RayanCustomer> builder)
        {
            BaseConfiguration<RayanCustomer>.Configure(builder);
            builder.ToTable("RayanCustomers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.AccountNumber).IsUnicode(true).HasMaxLength(50);
            builder.Property(c => c.NationalIdentifier).IsUnicode(false).HasMaxLength(15);
            builder.Property(c => c.CreationDate).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.BranchName).IsUnicode(true).HasMaxLength(100);
            
            builder.Property(c => c.BourseCode).IsUnicode(true).HasMaxLength(11);
            builder.Property(c => c.Personality).IsUnicode(true).HasMaxLength(25);
            builder.Property(c => c.Representative).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.FirstName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.LastName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.BirthCertificationNumber).IsUnicode(true).HasMaxLength(50);
            builder.Property(c => c.BirthDate).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.CellPhone).HasMaxLength(50); //.IsUnicode(false)

            builder.HasIndex(c => new { c.FundId, c.CustomerId }).IsUnique();
            builder.HasIndex(c => c.CellPhone);
        }

    }
}
