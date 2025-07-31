using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Core.Constants;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Providers.EntityFramework.Contexts
{
    public partial class FundContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FundContext()
        {

        }

        public FundContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<BankPayment> BankPayments { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Fund> Funds { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderHistory> OrderHistories { get; set; }
        public virtual DbSet<Limitation> Limitations { get; set; }
        public virtual DbSet<LimitationComponent> LimitationComponents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
#endif

            base.OnConfiguring(optionsBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var trackableEntries = ChangeTracker.Entries<ITrackable>();
            var _mapper = ServiceLocator.GetService<IMapper>();

            foreach (var trackableEntry in trackableEntries)
            {
                if (trackableEntry.State == EntityState.Added)
                {
                    trackableEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.Now;
                    trackableEntry.Property(e => e.CreatedById).CurrentValue =  GetUserId();
                }
                else if (trackableEntry.State == EntityState.Modified)
                {
                    trackableEntry.Property(e => e.ModifiedAt).CurrentValue = DateTime.Now;
                    trackableEntry.Property(e => e.ModifiedById).CurrentValue =  GetUserId();
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public long GetUserId()
        {
            long customerId = 0;
            if (_httpContextAccessor.HttpContext is null)
                return customerId;

            var customerIdParam = _httpContextAccessor.HttpContext.Request.Headers[GlobalConstants.CustomerIdKey].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(customerIdParam))
                return 0;

            if (!long.TryParse(customerIdParam, out customerId))
            {
                var ids = customerIdParam.ToString().Split(',');
                customerId = Convert.ToInt64(ids.First());
            }
            return customerId;
        }
    }
}
