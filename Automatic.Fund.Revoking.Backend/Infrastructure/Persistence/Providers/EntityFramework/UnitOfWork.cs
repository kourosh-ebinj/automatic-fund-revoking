using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Persistence.Providers.EntityFramework.Contexts;
using System;
using Domain.Abstractions;
using Application.Services.Abstractions.Persistence;

namespace Infrastructure.Persistence.Providers.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _ignoreSaveChanges = false;
        public FundContext DbContext { get; init; }

        public UnitOfWork(FundContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task SaveChanges(DateTime? trackableDateTime = null, CancellationToken cancellationToken = default)
        {
            if (_ignoreSaveChanges) return;

            //DetachValueObjects();

            if (!DbContext.ChangeTracker.HasChanges()) return;

            await DbContext.SaveChangesAsync();
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default) =>
            await SaveChanges(null, cancellationToken);

        public async Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default) =>
            await SaveChanges(async () =>
            {
                await func();
                return 0;
            }, ignoreSaveChanges);

        public async Task<T> SaveChanges<T>(Func<Task<T>> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default) =>
            await SaveChanges(async transaction => await func(), null, ignoreSaveChanges, cancellationToken);

        private async Task<T> SaveChanges<T>(Func<IDbContextTransaction, Task<T>> func, IDbContextTransaction transaction, bool ignoreSaveChanges = true,
                                             CancellationToken cancellationToken = default)
        {
            try
            {
                _ignoreSaveChanges = ignoreSaveChanges;
                var result = await func(transaction);
                _ignoreSaveChanges = false;

                //DetachValueObjects();

                await SaveChanges(cancellationToken);

                return result;
            }
            finally
            {
                _ignoreSaveChanges = false;
            }
        }

        public virtual async Task<int> ExecuteSql(string query, params object[] parameters)
        {
            return await DbContext.Database.ExecuteSqlRawAsync(query, parameters);
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
