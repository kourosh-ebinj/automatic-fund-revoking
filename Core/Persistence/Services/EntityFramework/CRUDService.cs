using System;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Core;
using Core.Abstractions;
using Persistence.Abstractions.EntityFramework;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Core.Entities;
using Core.Abstractions.Entities;
using Persistence.Providers.EntityFramework;
using Core.Models;

namespace Persistence.Services.EntityFramework
{
    public class CRUDService<T> : IDisposable, ICRUDService<T> where T : EntityBase, new()
    {
        public IUnitOfWork _unitOfWork { get; set; }
        protected readonly IGuard _guard;
        protected readonly IMapper _mapper;
        protected readonly ApplicationSettingModel _applicationSetting;

        //public CRUDService()
        //{
        //    _unitOfWork = (UnitOfWork)ServiceLocator.GetService<IUnitOfWork>(); 
        //    _applicationSetting = ServiceLocator.GetService<ApplicationSettingExtenderModel>();
        //    _guard = ServiceLocator.GetService<IGuard>();
        //    _mapper = ServiceLocator.GetService<IMapper>();
        //}

        public CRUDService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _applicationSetting = ServiceLocator.GetService<ApplicationSettingModel>();
            _guard = ServiceLocator.GetService<IGuard>();
            _mapper = ServiceLocator.GetService<IMapper>();
        }

        public virtual ValueTask<T> Find(object id, CancellationToken cancellationToken = default)
        {
            return Find([id], cancellationToken);
        }

        public virtual async ValueTask<T> Find(object[] ids, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.DbContext.Set<T>().FindAsync(ids, cancellationToken);
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

        public virtual IQueryable<TEntity> FromSql<TEntity>(string query, params object[] parameters) where TEntity : EntityBase
        {
            return _unitOfWork.DbContext.Set<TEntity>().FromSqlRaw(query, parameters).AsNoTracking();
        }

        protected void ValidateEntity(T entity)
        {
            if (entity is IValidate validatingEntity)
            {
                validatingEntity.Validate();
                _guard.Assert(validatingEntity.ValidationErrors.Count() < 1, Core.Enums.ExceptionCodeEnum.BadRequest, "Domain validation failed.");
            }
        }

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
