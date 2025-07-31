using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstractions.Persistence.Providers.Dapper
{
    //public interface IRepository<TEntity, TConnectionString> where TEntity : EntityBase, new()
    //                                                              where TConnectionString : IConnectionString, new()
    //{
        //Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters, string connectionString, CancellationToken cancellationToken);
        //Task<IEnumerable<T>> QueryAsync<T>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString, CancellationToken cancellationToken);
        //Task<IEnumerable<TOutput>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TOutput>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString,
        //     Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TOutput> func, CancellationToken cancellationToken);
        //Task<IEnumerable<TOutput>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TOutput>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString,
        //     Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TOutput> func, CancellationToken cancellationToken);
        //Task<IEnumerable<TOutput>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TOutput>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString,
        //     Func<TFirst, TSecond, TThird, TFourth, TFifth, TOutput> func, CancellationToken cancellationToken);
        //Task<IEnumerable<TOutput>> QueryAsync<TFirst, TSecond, TThird, TFourth, TOutput>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString,
        //     Func<TFirst, TSecond, TThird, TFourth, TOutput> func, CancellationToken cancellationToken);
        //Task<IEnumerable<TOutput>> QueryAsync<TFirst, TSecond, TThird, TOutput>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString,
        //     Func<TFirst, TSecond, TThird, TOutput> func, CancellationToken cancellationToken);
        //Task<IEnumerable<TOutput>> QueryAsync<TFirst, TSecond, TOutput>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString,
        //     Func<TFirst, TSecond, TOutput> func, CancellationToken cancellationToken, string splitOn = "Id");

        //Task<TDto> SelectAsync<TDto>(TId id, CancellationToken ct) where TDto : new();
        //Task<IEnumerable<TDto>> SelectAsync<TDto>(Dictionary<string, object> whereAndModel = null, Dictionary<string, object> whereOrModel = null, CancellationToken ct = default) where TDto : new();
        //Task<int> InsertAsync(TEntity model, CancellationToken ct);
        //Task<int> UpdateAsync(TId id, object model, CancellationToken ct);
        //Task<int> DeleteAsync(TId id, CancellationToken ct);


        //Task<IEnumerable<TDto>> ExecuteStoredProcedureAsync<TDto>(string spName, object[] args);

        //Task<T> ExecuteScalarAsync<T>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString, CancellationToken cancellationToken);
        //Task<int> ExecuteAsync(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString, CancellationToken cancellationToken);
        //Task<T> QueryFirstAsync<T>(string query, IEnumerable<KeyValuePair<string, object>> parameters, string connectionString, CancellationToken cancellationToken);

    //}

}
