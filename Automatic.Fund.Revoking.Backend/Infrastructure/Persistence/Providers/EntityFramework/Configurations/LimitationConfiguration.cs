using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Application.Models.Responses;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class LimitationConfiguration : IEntityTypeConfiguration<Limitation>
    {
        public void Configure(EntityTypeBuilder<Limitation> builder)
        {

            BaseConfiguration<Limitation>.Configure(builder);
            builder.ToTable("Limitations");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).HasMaxLength(100);
            builder.HasMany(a => a.LimitationComponents).WithOne(a => a.Limitation).HasForeignKey(f => f.LimitationId);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            //builder.ToTable(e => e.IsMemoryOptimized());

            builder.HasMany(e => e.LimitationComponents).WithOne(e => e.Limitation).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.FundId);

            builder.HasData(
            new List<Limitation>()
            {
                new Limitation() { Id = 1, FundId = 1, Title = "پیش از ثبت سفارش", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PreOrdering, },
                new Limitation() { Id = 2, FundId = 1, Title = "پیش از پرداخت", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PrePayment,  },
                new Limitation() { Id = 3, FundId = 2, Title = "پیش از ثبت سفارش", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PreOrdering, },
                new Limitation() { Id = 4, FundId = 2, Title = "پیش از پرداخت", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PrePayment,  },
                new Limitation() { Id = 5, FundId = 3, Title = "پیش از ثبت سفارش", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PreOrdering, },
                new Limitation() { Id = 6, FundId = 3, Title = "پیش از پرداخت", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PrePayment,  },
                new Limitation() { Id = 7, FundId = 4, Title = "پیش از ثبت سفارش", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PreOrdering, },
                new Limitation() { Id = 8, FundId = 4, Title = "پیش از پرداخت", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PrePayment,  },
                new Limitation() { Id = 9, FundId = 5, Title = "پیش از ثبت سفارش", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PreOrdering, },
                new Limitation() { Id = 10, FundId = 5, Title = "پیش از پرداخت", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PrePayment,  },
                new Limitation() { Id = 11, FundId = 6, Title = "پیش از ثبت سفارش", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PreOrdering, },
                new Limitation() { Id = 12, FundId = 6, Title = "پیش از پرداخت", LimitationTypeId = Domain.Enums.LimitationTypeEnum.PrePayment,  },
            }
            );
        }

    }
}
