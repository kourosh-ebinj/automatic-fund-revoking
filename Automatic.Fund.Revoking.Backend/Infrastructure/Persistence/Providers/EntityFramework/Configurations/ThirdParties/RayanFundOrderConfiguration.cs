using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.ThirdParties;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations.ThirdParties
{
    public class RayanFundOrderConfiguration : IEntityTypeConfiguration<RayanFundOrder>
    {
        public void Configure(EntityTypeBuilder<RayanFundOrder> builder)
        {
            BaseConfiguration<RayanFundOrder>.Configure(builder);
            builder.ToTable("RayanFundOrders");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.customerAccountNumber).IsUnicode(false).HasMaxLength(50);
            builder.Property(c => c.CustomerName).IsUnicode(true).HasMaxLength(200);
            builder.Property(c => c.NationalCode).IsUnicode(false).HasMaxLength(15);
            builder.Property(c => c.FoStatusName).IsUnicode(true).HasMaxLength(25);
            builder.Property(c => c.FundOrderNumber).IsUnicode(false).HasMaxLength(25);
            builder.Property(c => c.OrderDate).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.CreationDate).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.CreationTime).IsUnicode(false).HasMaxLength(5);
            builder.Property(c => c.ModificationDate).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.ModificationTime).IsUnicode(false).HasMaxLength(5);
            builder.Property(c => c.LicenseDate).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.BranchName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.AppuserName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.OrderType).IsUnicode(true).HasMaxLength(25);
            builder.Property(c => c.FundIssueTypeName).IsUnicode(true).HasMaxLength(25);
            builder.Property(c => c.FundIssueOriginName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.OrderPaymentTypeName).IsUnicode(true).HasMaxLength(100);
            builder.Property(c => c.ReceiptNumber).IsUnicode(false).HasMaxLength(25);
            builder.Property(c => c.BranchCode).IsUnicode(false).HasMaxLength(25);
            builder.Property(c => c.DlNumber).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.LicenseNumber).IsUnicode(false).HasMaxLength(10);
            builder.Property(c => c.ReceiptComment).IsUnicode(false).HasMaxLength(250);
            builder.Property(c => c.VoucherNumber).IsUnicode(false).HasMaxLength(25);
            builder.Property(c => c.VoucherTempNumber).IsUnicode(false).HasMaxLength(25);
            builder.Property(c => c.UserName).IsUnicode(false).HasMaxLength(25);

            builder.HasIndex(c => c.FundOrderId).IsUnique();
        }

    }
}
