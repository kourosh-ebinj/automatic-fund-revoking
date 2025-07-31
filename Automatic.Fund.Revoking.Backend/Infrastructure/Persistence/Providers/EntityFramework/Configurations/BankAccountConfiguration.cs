using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Domain.Entities;
using Core.Constants;
using Domain.Enums;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            BaseConfiguration<BankAccount>.Configure(builder);
            builder.ToTable("BankAccounts");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.AccountNumber).HasMaxLength(20);
            builder.Property(c => c.ShebaNumber).HasMaxLength(27);
            builder.Property(c => c.Balance).HasColumnType(GlobalConstants.PriceSqlDataType);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasIndex(e => e.FundId);

            builder.HasData(
                new BankAccount() { Id = 1, AccountNumber = "415.8100.41581000.1", ShebaNumber = "IR510570041581041581000101", BankId = 71, FundId = 2, IsEnabled = false },
                new BankAccount() { Id = 2, AccountNumber = "203.110.1419864.1", ShebaNumber = "IR190570077700101016288301", BankId = 71, FundId = 2, IsEnabled = true },
                new BankAccount() { Id = 3, AccountNumber = "203.110.1419864.1", ShebaNumber = "IR190570077700101016288301", BankId = 16, FundId = 1, IsEnabled = true }
                );
        }

    }
}
