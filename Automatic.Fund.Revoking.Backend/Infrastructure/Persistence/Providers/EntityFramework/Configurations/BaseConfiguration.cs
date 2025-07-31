using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure.Persistence.Providers.EntityFramework.Configurations
{
    public class BaseConfiguration<T> where T : notnull, EntityBase
    {
        public static void Configure(EntityTypeBuilder<T> builder)
        {
            //builder
            //    .ToTable(builder.Property(e => e.TableName).Metadata.Name);
            builder.Ignore(e => e.ValidationErrors);
        }
    }

}
