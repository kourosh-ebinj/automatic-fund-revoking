using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class UserFundConfiguration : IEntityTypeConfiguration<UserFund>
    {
        public void Configure(EntityTypeBuilder<UserFund> builder)
        {
            BaseConfiguration<UserFund>.Configure(builder);
            builder.ToTable("UserFunds");

            builder.HasKey(c => c.Id);

            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => new { e.UserId, e.FundId }).IsUnique();

            builder.HasData(
                new UserFund() { Id = 1, UserId = 84282, FundId = 1 },
                new UserFund() { Id = 2, UserId = 84282, FundId = 2 },
                new UserFund() { Id = 3, UserId = 84282, FundId = 3 },
                new UserFund() { Id = 4, UserId = 84282, FundId = 4 },
                new UserFund() { Id = 5, UserId = 84282, FundId = 5 },
                new UserFund() { Id = 6, UserId = 84282, FundId = 6 },
                
                new UserFund() { Id = 7, UserId = 84283, FundId = 1 },
                new UserFund() { Id = 8, UserId = 84283, FundId = 2 }
                );
        }

    }
}
