using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Entities;

namespace Persistence.Abstractions.EntityFramework
{
    public interface ICRUDService<T> where T : EntityBase, new()
    {
        ValueTask<T> Find(object id, CancellationToken cancellationToken = default);
        ValueTask<T> Find(object[] ids, CancellationToken cancellationToken = default);

        IQueryable<T> GetQuery();
        IQueryable<TEntity> GetQueryAs<TEntity>();
        IQueryable<T> GetQueryAsTracking();
        Task<T> Create(T entity, CancellationToken cancellationToken = default);
        Task Create(ICollection<T> entities, CancellationToken cancellationToken = default);
        Task<T> Update(T entity, CancellationToken cancellationToken = default);
        Task Update(ICollection<T> entities, CancellationToken cancellationToken = default);
        Task Delete(T entity, CancellationToken cancellationToken = default);
        Task Delete(ICollection<T> entities, CancellationToken cancellationToken = default);
        IQueryable<TEntity> FromSql<TEntity>(string query, params object[] parameters) where TEntity : EntityBase;
    }

}
