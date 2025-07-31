using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using Domain.Entities;
using Domain.Enums;
using System.Diagnostics;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            BaseConfiguration<Bank>.Configure(builder);
            builder.ToTable("Banks");

            builder.HasKey(c => c.Id);
            //builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.Description).HasMaxLength(100);
            builder.Property(c => c.ProviderClassName).HasMaxLength(150).IsUnicode(false);

            builder.HasIndex(c => c.FundId);

            builder.HasMany(e => e.BankAccounts).WithOne(e => e.Bank).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.BankPayments).WithOne(e => e.DestinationBank).OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
            #region Fund 1 // امین آشنا
                new Bank() { FundId = 1, Id = 1, Name = "اقتصاد نوين", BackOfficeBankId = 1, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 2, Name = "تجارت", BackOfficeBankId = 2, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 3, Name = "توسعه صادرات", BackOfficeBankId = 8, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 4, Name = "رفاه كارگران", BackOfficeBankId = 4, ProviderClassName = "Infrastructure.Services.ThirdParties.Banks.RefahBankingProviderService, Infrastructure", IsEnabled = false },
                new Bank() { FundId = 1, Id = 5, Name = "سامان", BackOfficeBankId = 5, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 6, Name = "سرمايه", BackOfficeBankId = 6, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 7, Name = "سپه", BackOfficeBankId = 7, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 8, Name = "صادرات", BackOfficeBankId = 0, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 9, Name = "صنعت و معدن", BackOfficeBankId = 9, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 10, Name = "مسكن", BackOfficeBankId = 10, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 11, Name = "ملت", BackOfficeBankId = 11, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 12, Name = "ملي", BackOfficeBankId = 12, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 13, Name = "موسسه اعتباري بنياد", BackOfficeBankId = 13, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 14, Name = "قوامين", BackOfficeBankId = 14, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 15, Name = "پارسيان", BackOfficeBankId = 15, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 16, Name = "پاسارگاد", BackOfficeBankId = 16, ProviderClassName = "Infrastructure.Services.ThirdParties.Banks.PasargadBankingProviderService, Infrastructure", IsEnabled = true },
                new Bank() { FundId = 1, Id = 17, Name = "كارآفرين", BackOfficeBankId = 17, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 18, Name = "كشاورزي", BackOfficeBankId = 18, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 19, Name = "شهر", BackOfficeBankId = 19, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 20, Name = "مركزي", BackOfficeBankId = 20, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 21, Name = "پست بانك", BackOfficeBankId = 21, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 22, Name = "موسسه اعتباري مهر", BackOfficeBankId = 22, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 23, Name = "انصار", BackOfficeBankId = 23, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 24, Name = "سينا", BackOfficeBankId = 24, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 25, Name = "صندوق تعاون", BackOfficeBankId = 25, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 26, Name = "موسسه اعتباري توسعه", BackOfficeBankId = 26, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 27, Name = "موسسه مالي مولي الموحدين", BackOfficeBankId = 27, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 28, Name = "موسسه اعتباری ملل", BackOfficeBankId = 28, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 29, Name = "موسسه اعتباري ثامن الائمه ", BackOfficeBankId = 29, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 30, Name = "آينده", BackOfficeBankId = 30, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 31, Name = "تعاوني اعتبار صالحين", BackOfficeBankId = 32, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 32, Name = "دي", BackOfficeBankId = 33, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 33, Name = "قرض الحسنه باب الحوائج ", BackOfficeBankId = 34, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 34, Name = "گردشگري", BackOfficeBankId = 35, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 35, Name = "توسعه تعاون", BackOfficeBankId = 36, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 36, Name = "ايران زمين", BackOfficeBankId = 37, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 37, Name = "موسسه اعتباري پيشگامان", BackOfficeBankId = 38, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 38, Name = "مؤسسه مالي و اعتباري آتي", BackOfficeBankId = 40, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 39, Name = "موسسه اعتباري ثامن الحجج", BackOfficeBankId = 41, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 40, Name = "موسسه مالي و اعتباري صالحين", BackOfficeBankId = 42, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 41, Name = "حكمت ايرانيان", BackOfficeBankId = 43, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 42, Name = "موسسه مالي و اعتباري فردوسي", BackOfficeBankId = 44, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 43, Name = "خاورميانه", BackOfficeBankId = 45, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 44, Name = "موسسه مالي و اعتباري رضوي", BackOfficeBankId = 46, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 45, Name = "قرض الحسنه مهر ايران", BackOfficeBankId = 47, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 46, Name = "قرض الحسنه رسالت", BackOfficeBankId = 48, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 47, Name = "مهر اقتصاد", BackOfficeBankId = 49, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 48, Name = "موسسه مالي و اعتباري کوثر", BackOfficeBankId = 50, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 49, Name = "مبنا", BackOfficeBankId = 51, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 50, Name = "پرشين", BackOfficeBankId = 52, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 51, Name = "موسسه اعتباری افضل توس", BackOfficeBankId = 54, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 52, Name = "موسسه اعتباری نور", BackOfficeBankId = 55, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 53, Name = "بانک مشترک ايران و ونزوئلا", BackOfficeBankId = 56, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 54, Name = "عابر بانك", BackOfficeBankId = 99, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 1, Id = 55, Name = "نامشخص", BackOfficeBankId = 100, ProviderClassName = "", IsEnabled = false },
            #endregion

            #region Fund 2 // حکمت
                new Bank() { FundId = 2, Id = 56, Name = "اقتصاد نوين", BackOfficeBankId = 1, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 57, Name = "تجارت", BackOfficeBankId = 2, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 58, Name = "توسعه صادرات", BackOfficeBankId = 3, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 59, Name = "رفاه كارگران", BackOfficeBankId = 4, ProviderClassName = "Infrastructure.Services.ThirdParties.Banks.RefahBankingProviderService, Infrastructure", IsEnabled = false },
                new Bank() { FundId = 2, Id = 60, Name = "سامان", BackOfficeBankId = 5, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 61, Name = "سرمايه", BackOfficeBankId = 6, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 62, Name = "سپه", BackOfficeBankId = 7, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 63, Name = "صادرات", BackOfficeBankId = 8, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 64, Name = "صنعت و معدن", BackOfficeBankId = 9, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 65, Name = "مسكن", BackOfficeBankId = 10, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 66, Name = "ملت", BackOfficeBankId = 11, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 67, Name = "ملي", BackOfficeBankId = 12, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 68, Name = "موسسه اعتباري بنياد", BackOfficeBankId = 13, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 69, Name = "قوامين", BackOfficeBankId = 14, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 70, Name = "پارسيان", BackOfficeBankId = 15, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 71, Name = "پاسارگاد", BackOfficeBankId = 16, ProviderClassName = "Infrastructure.Services.ThirdParties.Banks.PasargadBankingProviderService, Infrastructure", IsEnabled = true },
                new Bank() { FundId = 2, Id = 72, Name = "كارآفرين", BackOfficeBankId = 17, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 73, Name = "كشاورزي", BackOfficeBankId = 18, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 74, Name = "شهر", BackOfficeBankId = 19, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 75, Name = "مركزي", BackOfficeBankId = 20, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 76, Name = "پست بانك", BackOfficeBankId = 21, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 77, Name = "موسسه اعتباري مهر", BackOfficeBankId = 22, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 78, Name = "انصار", BackOfficeBankId = 23, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 79, Name = "سينا", BackOfficeBankId = 24, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 80, Name = "صندوق تعاون", BackOfficeBankId = 25, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 81, Name = "موسسه اعتباري توسعه", BackOfficeBankId = 26, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 82, Name = "موسسه مالي مولي الموحدين", BackOfficeBankId = 27, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 83, Name = "موسسه اعتباری ملل", BackOfficeBankId = 28, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 84, Name = "موسسه اعتباري ثامن الائمه ", BackOfficeBankId = 29, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 85, Name = "آينده", BackOfficeBankId = 30, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 86, Name = "تعاوني اعتبار صالحين", BackOfficeBankId = 32, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 87, Name = "دي", BackOfficeBankId = 33, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 88, Name = "قرض الحسنه باب الحوائج ", BackOfficeBankId = 34, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 89, Name = "گردشگري", BackOfficeBankId = 35, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 90, Name = "توسعه تعاون", BackOfficeBankId = 36, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 91, Name = "ايران زمين", BackOfficeBankId = 37, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 92, Name = "موسسه اعتباري پيشگامان", BackOfficeBankId = 38, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 93, Name = "مؤسسه مالي و اعتباري آتي", BackOfficeBankId = 40, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 94, Name = "موسسه اعتباري ثامن الحجج", BackOfficeBankId = 41, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 95, Name = "موسسه مالي و اعتباري صالحين", BackOfficeBankId = 42, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 96, Name = "حكمت ايرانيان", BackOfficeBankId = 43, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 97, Name = "موسسه مالي و اعتباري فردوسي", BackOfficeBankId = 44, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 98, Name = "خاورميانه", BackOfficeBankId = 45, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 99, Name = "موسسه مالي و اعتباري رضوي", BackOfficeBankId = 46, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 100, Name = "قرض الحسنه مهر ايران", BackOfficeBankId = 47, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 101, Name = "قرض الحسنه رسالت", BackOfficeBankId = 48, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 102, Name = "مهر اقتصاد", BackOfficeBankId = 49, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 103, Name = "موسسه مالي و اعتباري کوثر", BackOfficeBankId = 50, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 104, Name = "مبنا", BackOfficeBankId = 51, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 105, Name = "پرشين", BackOfficeBankId = 52, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 106, Name = "موسسه اعتباری افضل توس", BackOfficeBankId = 53, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 107, Name = "موسسه اعتباری نور", BackOfficeBankId = 55, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 108, Name = "بانک مشترک ايران و ونزوئلا", BackOfficeBankId = 56, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 109, Name = "عابر بانك", BackOfficeBankId = 99, ProviderClassName = "", IsEnabled = false },
                new Bank() { FundId = 2, Id = 110, Name = "نامشخص", BackOfficeBankId = 100, ProviderClassName = "", IsEnabled = false }
                #endregion

                );
        }

    }
}
