using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Core.Constants;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class BankPaymentConfiguration : IEntityTypeConfiguration<BankPayment>
    {
        public void Configure(EntityTypeBuilder<BankPayment> builder)
        {
            BaseConfiguration<BankPayment>.Configure(builder);
            builder.ToTable("BankPayments");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Description).HasMaxLength(100);
            builder.Property(c => c.BankUniqueId).HasMaxLength(70).IsUnicode(false);
            builder.Property(c => c.DestinationShebaNumber).HasMaxLength(27).IsUnicode(false);
            builder.Property(c => c.TotalAmount).HasColumnType(GlobalConstants.PriceSqlDataType);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            //builder.HasOne(e => e.Order).WithOne(e => e.BankPayment).OnDelete(DeleteBehavior.Restrict);

        }

    }
}
