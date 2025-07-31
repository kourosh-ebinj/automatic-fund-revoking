using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Domain.Entities;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class CustomerBankAccountConfiguration : IEntityTypeConfiguration<CustomerBankAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerBankAccount> builder)
        {
            BaseConfiguration<CustomerBankAccount>.Configure(builder);
            builder.ToTable("CustomerBankAccounts");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.AccountNumber).HasMaxLength(20);
            builder.Property(c => c.ShebaNumber).HasMaxLength(27);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

        }

    }
}
