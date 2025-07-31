using Domain.Entities;
using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Core;
using Core.Abstractions;
using Application.Services.Abstractions;
using Application.Models;
using Infrastructure.Persistence.Providers.EntityFramework;
using Domain.Abstractions;
using Application.Services.Abstractions.Persistence;

namespace Infrastructure.Services.EntityFramework
{
    public class CRUDService<T> : IDisposable, ICRUDService<T> where T : EntityBase, new()
    {
        public UnitOfWork _unitOfWork { get; set; }
        protected readonly IGuard _guard;
        protected readonly IMapper _mapper;
        protected readonly ApplicationSettingExtenderModel _applicationSetting;

        //public CRUDService()
        //{
        //    _unitOfWork = (UnitOfWork)ServiceLocator.GetService<IUnitOfWork>(); 
        //    _applicationSetting = ServiceLocator.GetService<ApplicationSettingExtenderModel>();
        //    _guard = ServiceLocator.GetService<IGuard>();
        //    _mapper = ServiceLocator.GetService<IMapper>();
        //}

        public CRUDService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _applicationSetting = ServiceLocator.GetService<ApplicationSettingExtenderModel>();
            _guard = ServiceLocator.GetService<IGuard>();
            _mapper = ServiceLocator.GetService<IMapper>();
        }

        public virtual ValueTask<T> Find(object id, CancellationToken cancellationToken = default)
        {
            return Find([id], cancellationToken);
        }

        public virtual ValueTask<T> Find(object[] ids, CancellationToken cancellationToken = default)
        {
            return _unitOfWork.DbContext.Set<T>().FindAsync(ids, cancellationToken);
        }

        public virtual IQueryable<T> GetQuery()
        {
            return _unitOfWork.DbContext.Set<T>().AsNoTracking().AsQueryable();
        }

        public virtual IQueryable<TEntity> GetQueryAs<TEntity>()
        {
            return _unitOfWork.DbContext.Set<T>().AsNoTracking().OfType<TEntity>().AsQueryable();
        }

        public virtual IQueryable<T> GetQueryAsTracking()
        {
            return _unitOfWork.DbContext.Set<T>().AsQueryable();
        }

        public virtual async Task<T> Create(T entity, CancellationToken cancellationToken = default)
        {
            ValidateEntity(entity);

            return _unitOfWork.DbContext.Set<T>().Add(entity).Entity;
        }

        public virtual async Task Create(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                ValidateEntity(entity);

            await _unitOfWork.DbContext.Set<T>().AddRangeAsync(entities);
        }

        public virtual async Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            ValidateEntity(entity);
            
            return _unitOfWork.DbContext.Set<T>().Update(entity).Entity;
        }

        public virtual async Task Update(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                ValidateEntity(entity);

            _unitOfWork.DbContext.Set<T>().UpdateRange(entities);
        }

        public virtual async Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.DbContext.Entry(entity).State = EntityState.Deleted;

            _unitOfWork.DbContext.Set<T>().Remove(entity);
        }

        public virtual async Task Delete(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                _unitOfWork.DbContext.Entry(entity).State = EntityState.Deleted;

            _unitOfWork.DbContext.Set<T>().RemoveRange(entities);
        }

        public virtual IQueryable<T> FromSql(string query)
        {
            return _unitOfWork.DbContext.Set<T>().FromSqlRaw(query).AsNoTracking();
        }

        public virtual IQueryable<TEntity> FromSql<TEntity>(string query) where TEntity : class
        {
            return _unitOfWork.DbContext.Set<TEntity>().FromSqlRaw(query).AsNoTracking();
        }

        protected void ValidateEntity(T entity)
        {
            if (entity is IValidate validatingEntity)
            {
                validatingEntity.Validate();
                _guard.Assert(validatingEntity.ValidationErrors.Count() < 1, Core.Enums.ExceptionCodeEnum.BadRequest, "Domain validation failed.");
            }
        }


        #region SaveChanges 


        //public async Task SaveChanges(DateTime? trackableDateTime = null, CancellationToken cancellationToken = default)
        //{
        //    if (_ignoreSaveChanges) return;

        //    var dateTime = trackableDateTime ?? DateTime.Now;
        //    //ProcessTrackable(dateTime);
        //    //ProcessRemovable(useTrackableDateTime);

        //    //DetachValueObjects();

        //    if (_unitOfWork.DbContext.ChangeTracker.HasChanges())
        //        await _unitOfWork.DbContext.SaveChangesAsync();

        //    //DbContext.ChangeTracker.Clear();
        //}

        //public async Task SaveChanges(CancellationToken cancellationToken = default) =>
        //    await SaveChanges(null, cancellationToken);

        //public async Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default) =>
        //    await SaveChanges(async () =>
        //    {
        //        await func();
        //        return 0;
        //    }, ignoreSaveChanges);

        //public async Task<T> SaveChanges<T>(Func<Task<T>> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default) =>
        //    await SaveChanges(async transaction => await func(), null, ignoreSaveChanges, cancellationToken);

        //private async Task<T> SaveChanges<T>(Func<IDbContextTransaction, Task<T>> func, IDbContextTransaction transaction, bool ignoreSaveChanges = true,
        //                                     CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        _ignoreSaveChanges = ignoreSaveChanges;
        //        var result = await func(transaction);
        //        _ignoreSaveChanges = false;

        //        //DetachValueObjects();

        //        await SaveChanges(cancellationToken);

        //        return result;
        //    }
        //    finally
        //    {
        //        _ignoreSaveChanges = false;
        //    }
        //}

        //private void ProcessTrackable(DateTime trackableDateTime)
        //{
        //    var entries = _unitOfWork.DbContext.ChangeTracker.Entries().Where(x =>
        //        x.Entity is ITrackable && (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();
        //    foreach (var entry in entries)
        //    {
        //        var trackable = entry.Entity as ITrackable;

        //        var principalId = 0; // AuthPrincipalService.GetCurrentPrincipalIdIfLogin();

        //        switch (entry.State)
        //        {
        //            case EntityState.Modified:
        //                trackable.ModifiedAt = trackableDateTime;
        //                trackable.ModifiedById = principalId;
        //                break;

        //            case EntityState.Added:
        //                trackable.CreatedAt = trackableDateTime;
        //                trackable.CreatedById = principalId;
        //                trackable.ModifiedAt = null;
        //                break;
        //        }
        //    }
        //}

        #endregion

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _unitOfWork.DbContext.Dispose();
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
