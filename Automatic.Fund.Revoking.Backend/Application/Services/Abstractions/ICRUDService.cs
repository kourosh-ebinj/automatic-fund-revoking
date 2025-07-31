using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Abstractions
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
        IQueryable<T> FromSql(string query);
        IQueryable<TEntity> FromSql<TEntity>(string query) where TEntity : class;

        //Task SaveChanges(DateTime? trackableDateTime = null, CancellationToken cancellationToken = default);
        //Task SaveChanges(CancellationToken cancellationToken = default);
        //Task SaveChanges(Func<Task> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default);
        //Task<T> SaveChanges<T>(Func<Task<T>> func, bool ignoreSaveChanges = true, CancellationToken cancellationToken = default);

    }

}
