using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class SagaTransactionConfiguration : IEntityTypeConfiguration<SagaTransaction>
    {
        public void Configure(EntityTypeBuilder<SagaTransaction> builder)
        {
            BaseConfiguration<SagaTransaction>.Configure(builder);
            builder.ToTable("SagaTransactions");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Description).HasMaxLength(200);

            builder.HasIndex(c => c.OrderId).IsDescending().IsUnique();

        }

    }
}
