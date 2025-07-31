using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Extensions;

namespace Presentation.Extensions
{
    public static class WebHostExtensions
    {

        public static IHost Migrate<TDbContext>(this IHost host) where TDbContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider.GetRequiredService<TDbContext>();

            dbContext.Database.Migrate();
            dbContext.SaveChanges();
            return host;
        }
    }
}
