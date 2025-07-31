using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.ThirdParties;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations.ThirdParties
{
    public class PasargadBankAccountDetailConfiguration : IEntityTypeConfiguration<PasargadBankAccountDetail>
    {
        public void Configure(EntityTypeBuilder<PasargadBankAccountDetail> builder)
        {
            BaseConfiguration<PasargadBankAccountDetail>.Configure(builder);
            builder.ToTable("PasargadBankAccountDetails");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ScApiKeyKYC).IsUnicode(false).HasMaxLength(50);
            builder.Property(c => c.ScApiKeyInternal).IsUnicode(false).HasMaxLength(50);
            builder.Property(c => c.ScApiKeyAccountBalance).IsUnicode(false).HasMaxLength(50);
            builder.Property(c => c.ScApiKeyPaya).IsUnicode(false).HasMaxLength(50);
            builder.Property(c => c.ScApiKeySatna).IsUnicode(false).HasMaxLength(50);

            builder.HasData(
                new PasargadBankAccountDetail()
                {
                    Id = 1,
                    BankAccountId = 2,
                    ScApiKeyAccountBalance = "0f8d375954b64fc1a4a92b33e95c5c87.XzIwMjQ3",
                    ScApiKeyInternal = "edd7c1468e2949d1871f191c369084f3.XzIwMjQ1",
                    ScApiKeyPaya = "6e036d9932864ded945807f7132ae4da.XzIwMjQ3",
                    ScApiKeySatna = "9414ea350a234ece99af089647841cf9.XzIwMjQ3",
                    ScApiKeyKYC = "e36f3972a7654900bfab231e1225951e.XzIwMjQ3",
                },
                new PasargadBankAccountDetail()
                {
                    Id = 2,
                    BankAccountId = 3,
                    ScApiKeyAccountBalance = "0f8d375954b64fc1a4a92b33e95c5c87.XzIwMjQ3",
                    ScApiKeyInternal = "edd7c1468e2949d1871f191c369084f3.XzIwMjQ1",
                    ScApiKeyPaya = "6e036d9932864ded945807f7132ae4da.XzIwMjQ3",
                    ScApiKeySatna = "9414ea350a234ece99af089647841cf9.XzIwMjQ3",
                    ScApiKeyKYC = "e36f3972a7654900bfab231e1225951e.XzIwMjQ3",
                }
                );
        }

    }
}
