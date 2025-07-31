using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Providers.EntityFramework.Contexts
{
    //public class SampleContextFactory : IDbContextFactory<SampleContext>
    //{
    //    private readonly IDbContextFactory<SampleContext> _pooledFactory;
    //    //private readonly MyRequestTenant _tenant;

    //    //public EventDataContext Create(DbContextFactoryOptions options)
    //    //{
    //    //    var builder = new DbContextOptionsBuilder<EventDataContext>();
    //    //    builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SampleApiDatabase;Trusted_Connection=True;");
    //    //    return new EventDataContext(builder.Options);
    //    //}

    //    public SampleContextFactory(IDbContextFactory<SampleContext> pooledFactory) //, MyRequestTenant tenant
    //    {
    //        _pooledFactory = pooledFactory;
    //        //_tenant = tenant;
    //    }

    //    public SampleContext CreateDbContext()
    //    {
    //        var context = _pooledFactory.CreateDbContext();
    //        //context.TenantID = _tenant.TenantID;

    //        return context;
    //    }
    //}
}
