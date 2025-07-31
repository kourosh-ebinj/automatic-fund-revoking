using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class IBanKYCConfigurationConfiguration : IEntityTypeConfiguration<IBanKYC>
    {
        public void Configure(EntityTypeBuilder<IBanKYC> builder)
        {
            BaseConfiguration<IBanKYC>.Configure(builder);
            builder.ToTable("IBanKYCs");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.IBan).IsUnicode(false).HasMaxLength(26);
            builder.Property(c => c.IsKYCCompliant);

            builder.HasIndex(e => e.IBan).IsUnique();

        }

    }
}
