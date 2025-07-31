using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain.Entities.Audit;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations.Audit
{
    public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
    {
        public void Configure(EntityTypeBuilder<OrderHistory> builder)
        {
            BaseConfiguration<OrderHistory>.Configure(builder);
            builder.ToTable("OrderHistories");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");


        }

    }
}
