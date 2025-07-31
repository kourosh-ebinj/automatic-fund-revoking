using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Domain.Entities;
using System.Collections.Generic;
using Domain.Enums;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class LimitationComponentConfiguration : IEntityTypeConfiguration<LimitationComponent>
    {
        public void Configure(EntityTypeBuilder<LimitationComponent> builder)
        {
            BaseConfiguration<LimitationComponent>.Configure(builder);
            builder.ToTable("LimitationComponents");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).HasMaxLength(100);
            builder.Property(c => c.Value).HasMaxLength(100);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Error).HasMaxLength(100);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasIndex(e => e.LimitationId);

            //builder.ToTable(e => e.IsMemoryOptimized());

            builder.HasData(
                new LimitationComponent { Id = 1, Title = "چک کردن حداکثر تعداد واحد", Value = "1000", Enabled = false, Error = "تعداد واحدها بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxUnits, LimitationId = 1 },
                new LimitationComponent { Id = 2, Title = "چک کردن حداقل تعداد واحد", Value = "1", Enabled = false, Error = "تعداد واحدها کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinUnits, LimitationId = 1 },
                new LimitationComponent { Id = 3, Title = "چک کردن حداقل مبلغ درخواست", Value = "100000", Enabled = false, Error = "مبلغ درخواست کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinAmount, LimitationId = 1 },
                new LimitationComponent { Id = 4, Title = "چک کردن حداکثر مبلغ درخواست", Value = "1000000000", Enabled = false, Error = "مبلغ درخواست بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxAmount, LimitationId = 1 },
                new LimitationComponent { Id = 5, Title = "لیست کاربران مجاز", Value = "[158,31130]", Enabled = true, Error = "ارایه سرویس به کاربر جاری ممکن نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.CustomerWhitelist, LimitationId = 1 },
                new LimitationComponent { Id = 6, Title = "لیست سامانه های مجاز", Value = "[\"fund_site\"]", Enabled = true, Error = "ارایه سرویس به سامانه جاری مجاز نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.AppWhitelist, LimitationId = 1 },
                new LimitationComponent { Id = 7, Title = "چک کردن موجودی حساب", Value = @"{ ""MinBalance"": 100000000 }", Enabled = true, Error = "موجودی حساب کافی نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.BankAccountBalance, LimitationId = 2 },

                new LimitationComponent { Id = 8, Title = "چک کردن حداکثر تعداد واحد", Value = "1000", Enabled = false, Error = "تعداد واحدها بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxUnits, LimitationId = 3 },
                new LimitationComponent { Id = 9, Title = "چک کردن حداقل تعداد واحد", Value = "1", Enabled = false, Error = "تعداد واحدها کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinUnits, LimitationId = 3 },
                new LimitationComponent { Id = 10, Title = "چک کردن حداقل مبلغ درخواست", Value = "100000", Enabled = false, Error = "مبلغ درخواست کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinAmount, LimitationId = 3 },
                new LimitationComponent { Id = 11, Title = "چک کردن حداکثر مبلغ درخواست", Value = "1000000000", Enabled = false, Error = "مبلغ درخواست بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxAmount, LimitationId = 3 },
                new LimitationComponent { Id = 12, Title = "لیست کاربران مجاز", Value = "[158,31130]", Enabled = true, Error = "ارایه سرویس به کاربر جاری ممکن نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.CustomerWhitelist, LimitationId = 3 },
                new LimitationComponent { Id = 13, Title = "لیست سامانه های مجاز", Value = "[\"fund_site\"]", Enabled = true, Error = "ارایه سرویس به سامانه جاری مجاز نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.AppWhitelist, LimitationId = 3 },
                new LimitationComponent { Id = 14, Title = "چک کردن موجودی حساب", Value = @"{ ""MinBalance"": 100000000 }", Enabled = true, Error = "موجودی حساب کافی نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.BankAccountBalance, LimitationId = 4 },

                new LimitationComponent { Id = 15, Title = "چک کردن حداکثر تعداد واحد", Value = "1000", Enabled = false, Error = "تعداد واحدها بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxUnits, LimitationId = 5 },
                new LimitationComponent { Id = 16, Title = "چک کردن حداقل تعداد واحد", Value = "1", Enabled = false, Error = "تعداد واحدها کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinUnits, LimitationId = 5 },
                new LimitationComponent { Id = 17, Title = "چک کردن حداقل مبلغ درخواست", Value = "100000", Enabled = false, Error = "مبلغ درخواست کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinAmount, LimitationId = 5 },
                new LimitationComponent { Id = 18, Title = "چک کردن حداکثر مبلغ درخواست", Value = "1000000000", Enabled = false, Error = "مبلغ درخواست بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxAmount, LimitationId = 5 },
                new LimitationComponent { Id = 19, Title = "لیست کاربران مجاز", Value = "[158,31130]", Enabled = true, Error = "ارایه سرویس به کاربر جاری ممکن نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.CustomerWhitelist, LimitationId = 5 },
                new LimitationComponent { Id = 20, Title = "لیست سامانه های مجاز", Value = "[\"fund_site\"]", Enabled = true, Error = "ارایه سرویس به سامانه جاری مجاز نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.AppWhitelist, LimitationId = 5 },
                new LimitationComponent { Id = 21, Title = "چک کردن موجودی حساب", Value = @"{ ""MinBalance"": 100000000 }", Enabled = true, Error = "موجودی حساب کافی نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.BankAccountBalance, LimitationId = 6 },

                new LimitationComponent { Id = 22, Title = "چک کردن حداکثر تعداد واحد", Value = "1000", Enabled = false, Error = "تعداد واحدها بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxUnits, LimitationId = 7 },
                new LimitationComponent { Id = 23, Title = "چک کردن حداقل تعداد واحد", Value = "1", Enabled = false, Error = "تعداد واحدها کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinUnits, LimitationId = 7 },
                new LimitationComponent { Id = 24, Title = "چک کردن حداقل مبلغ درخواست", Value = "100000", Enabled = false, Error = "مبلغ درخواست کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinAmount, LimitationId = 7 },
                new LimitationComponent { Id = 25, Title = "چک کردن حداکثر مبلغ درخواست", Value = "1000000000", Enabled = false, Error = "مبلغ درخواست بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxAmount, LimitationId = 7 },
                new LimitationComponent { Id = 26, Title = "لیست کاربران مجاز", Value = "[158,31130]", Enabled = true, Error = "ارایه سرویس به کاربر جاری ممکن نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.CustomerWhitelist, LimitationId = 7 },
                new LimitationComponent { Id = 27, Title = "لیست سامانه های مجاز", Value = "[\"fund_site\"]", Enabled = true, Error = "ارایه سرویس به سامانه جاری مجاز نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.AppWhitelist, LimitationId = 7 },
                new LimitationComponent { Id = 28, Title = "چک کردن موجودی حساب", Value = @"{ ""MinBalance"": 100000000 }", Enabled = true, Error = "موجودی حساب کافی نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.BankAccountBalance, LimitationId = 8 },

                new LimitationComponent { Id = 29, Title = "چک کردن حداکثر تعداد واحد", Value = "1000", Enabled = false, Error = "تعداد واحدها بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxUnits, LimitationId = 9 },
                new LimitationComponent { Id = 30, Title = "چک کردن حداقل تعداد واحد", Value = "1", Enabled = false, Error = "تعداد واحدها کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinUnits, LimitationId = 9 },
                new LimitationComponent { Id = 31, Title = "چک کردن حداقل مبلغ درخواست", Value = "100000", Enabled = false, Error = "مبلغ درخواست کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinAmount, LimitationId = 9 },
                new LimitationComponent { Id = 32, Title = "چک کردن حداکثر مبلغ درخواست", Value = "1000000000", Enabled = false, Error = "مبلغ درخواست بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxAmount, LimitationId = 9 },
                new LimitationComponent { Id = 33, Title = "لیست کاربران مجاز", Value = "[158,31130]", Enabled = true, Error = "ارایه سرویس به کاربر جاری ممکن نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.CustomerWhitelist, LimitationId = 9 },
                new LimitationComponent { Id = 34, Title = "لیست سامانه های مجاز", Value = "[\"fund_site\"]", Enabled = true, Error = "ارایه سرویس به سامانه جاری مجاز نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.AppWhitelist, LimitationId = 9 },
                new LimitationComponent { Id = 35, Title = "چک کردن موجودی حساب", Value = @"{ ""MinBalance"": 100000000 }", Enabled = true, Error = "موجودی حساب کافی نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.BankAccountBalance, LimitationId = 10 },

                new LimitationComponent { Id = 36, Title = "چک کردن حداکثر تعداد واحد", Value = "1000", Enabled = false, Error = "تعداد واحدها بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxUnits, LimitationId = 11 },
                new LimitationComponent { Id = 37, Title = "چک کردن حداقل تعداد واحد", Value = "1", Enabled = false, Error = "تعداد واحدها کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinUnits, LimitationId = 11 },
                new LimitationComponent { Id = 38, Title = "چک کردن حداقل مبلغ درخواست", Value = "100000", Enabled = false, Error = "مبلغ درخواست کمتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMinAmount, LimitationId = 11 },
                new LimitationComponent { Id = 39, Title = "چک کردن حداکثر مبلغ درخواست", Value = "1000000000", Enabled = false, Error = "مبلغ درخواست بیشتر از بازه تعیین شده است", LimitationComponentTypeId = LimitationComponentTypeEnum.FundCancellationMaxAmount, LimitationId = 11 },
                new LimitationComponent { Id = 40, Title = "لیست کاربران مجاز", Value = "[158,31130]", Enabled = true, Error = "ارایه سرویس به کاربر جاری ممکن نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.CustomerWhitelist, LimitationId = 11 },
                new LimitationComponent { Id = 41, Title = "لیست سامانه های مجاز", Value = "[\"fund_site\"]", Enabled = true, Error = "ارایه سرویس به سامانه جاری مجاز نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.AppWhitelist, LimitationId = 11 },
                new LimitationComponent { Id = 42, Title = "چک کردن موجودی حساب", Value = @"{ ""MinBalance"": 100000000 }", Enabled = true, Error = "موجودی حساب کافی نیست", LimitationComponentTypeId = LimitationComponentTypeEnum.BankAccountBalance, LimitationId = 12 }
            ); ;
        }

    }
}
