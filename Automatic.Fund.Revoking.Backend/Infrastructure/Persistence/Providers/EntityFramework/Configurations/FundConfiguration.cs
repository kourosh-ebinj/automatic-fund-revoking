using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class FundConfiguration : IEntityTypeConfiguration<Fund>
    {
        public void Configure(EntityTypeBuilder<Fund> builder)
        {
            BaseConfiguration<Fund>.Configure(builder);
            builder.ToTable("Funds");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(150);
            builder.Property(c => c.DsCode);

            builder.HasMany(e => e.RayanCustomers).WithOne(e => e.Fund).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.RayanFundOrders).WithOne(e => e.Fund).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.UserFunds).WithOne(e => e.Fund).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.Banks).WithOne(e => e.Fund).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Fund() { Id = 1, Name = "صندوق سرمایه گذاری امین آشنا ایرانیان", DsCode = 11049, IsEnabled = false },
                new Fund() { Id = 2, Name = "صندوق سرمایه گذاری حکمت آشنا ایرانیان", DsCode = 10784, IsEnabled = true },
                new Fund() { Id = 3, Name = "صندوق سرمایه گذاری سهم آشنا", DsCode = 11223, IsEnabled = false },
                new Fund() { Id = 4, Name = "صندوق سرمايه گذاری نيكوكاری جايزه علمی و فناوری پيامبر اعظم (ص)", DsCode = 11380, IsEnabled = false },
                new Fund() { Id = 5, Name = "صندوق سرمایه گذاری نماد آشنا ایرانیان", DsCode = 11915, IsEnabled = false },
                new Fund() { Id = 6, Name = "صندوق سرمایه گذاری کامیاب آشنا", DsCode = 126, IsEnabled = false }
                
                );
        }

    }
}
