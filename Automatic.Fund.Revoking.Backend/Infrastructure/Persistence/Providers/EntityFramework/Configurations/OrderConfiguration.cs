using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Core.Constants;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            BaseConfiguration<Order>.Configure(builder);
            builder.ToTable("Orders");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Title).HasMaxLength(100);
            builder.Property(c => c.CustomerFullName).HasMaxLength(100);
            builder.Property(c => c.OrderStatusDescription).HasMaxLength(100);
            builder.Property(c => c.CustomerAccountNumber).HasMaxLength(25).IsUnicode(false);
            builder.Property(c => c.CustomerAccountSheba).HasMaxLength(27).IsUnicode(false);
            builder.Property(c => c.CustomerNationalCode).HasMaxLength(13).IsUnicode(false);
            builder.Property(c => c.AppName).HasMaxLength(100);
            builder.Property(c => c.TotalAmount).HasColumnType(GlobalConstants.PriceSqlDataType);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasMany(e => e.OrderHistories).WithOne(e => e.Order).OnDelete(DeleteBehavior.Restrict);

        }

    }
}
