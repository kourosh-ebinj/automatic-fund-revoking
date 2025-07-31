using Core.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Services.Abstractions
{
    public interface IEntityFrameworkService<TEntity> // where TEntity : EntityBase<>
    {
        //Task<TEntity> GetAsync(TId id);
        //Task<TEntity> GetAsync(TId id, bool asNoTracking = true);
        //Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true);
        ////Task<TEntity> GetAsNoTrackingAsync(TId id);
        //Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        //Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true,
        //                             int pageNumber = 1, int pageSize = GlobalConstants.Default_PageSize);
        //void Add(TEntity entity);
        //void Add(IEnumerable<TEntity> entities);
        //void Edit(TEntity entity);
        ////void Delete(TEntity entity);
        //Task<int> CountAllAsync(CancellationToken cancellationToken);
        //Task<int> CountAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    }

}
