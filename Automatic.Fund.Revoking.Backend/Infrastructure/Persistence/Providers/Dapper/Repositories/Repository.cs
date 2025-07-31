using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.Providers.Dapper.Repositories
{

    //public class Repository<TEntity, TConnectionString> : IRepository<TEntity, TConnectionString>
    //                                                           where TEntity : EntityBase, new()
    //                                                           where TConnectionString : IConnectionString, new()
    //{
    //    private static readonly ConcurrentDictionary<Type, Tuple<string, Type>> _keyPropCache = new ConcurrentDictionary<Type, Tuple<string, Type>>();
    //    private static readonly List<string> _ignoreColumns = new List<string>();
    //    private readonly ReflectionHelper _reflectionHelper;

    //    static Repository()
    //    {
    //        //_ignoreColumns.Add(nameof(EntityBase<TId>.Id));
    //        //_ignoreColumns.Add(nameof(EntityBase<TId>.TableName));
    //        _ignoreColumns.Add(nameof(EntityBase.ValidationErrors));
    //    }

    //    public Repository()
    //    {
    //        _reflectionHelper = ServiceLocator.GetService<ReflectionHelper>();

    //    }


    //    //public async Task<int> InsertAsync(TEntity entity, CancellationToken ct)
    //    //{
    //    //    if (entity is IValidateDomain)
    //    //    {
    //    //        var validatingObject = (IValidateDomain)entity;
    //    //        validatingObject.ValidateDomain();
    //    //        if (validatingObject.ValidationErrors.Any())
    //    //            throw new DomainValidationException(validatingObject.ValidationErrors);
    //    //    }

    //    //    var obj = (object)entity;
    //    //    List<string> paramNames = GetParamNames(obj);
    //    //    paramNames.RemoveAll(e => _ignoreColumns.Contains(e));

    //    //    string cols = string.Join(",", paramNames);
    //    //    string colsParams = string.Join(",", paramNames.Select(p => "@" + p));

    //    //    var sql = $"insert {entity.TableName} ({cols}) values ({colsParams}) select cast(scope_identity() as int)";
    //    //    using (var connection = new SqlConnection(new TConnectionString().ConnectionString))
    //    //    {
    //    //        connection.Open();
    //    //        var result = await connection.ExecuteAsync(sql, entity);
    //    //        return result;
    //    //    }
    //    //}

    //    //public async Task<int> DeleteAsync(TId id, CancellationToken ct)
    //    //{
    //    //    var entity = new TEntity();
    //    //    var keyColumn = GetKeyProperty(entity);

    //    //    var sql = $"delete {new TEntity().TableName} where {keyColumn.Item1} = {id}";
    //    //    using (var connection = new SqlConnection(new TConnectionString().ConnectionString))
    //    //    {
    //    //        connection.Open();
    //    //        var result = await connection.ExecuteAsync(sql);
    //    //        return result;
    //    //    }
    //    //}

    //    //public async Task<int> UpdateAsync(TId id, object model, CancellationToken ct)
    //    //{
    //    //    var entity = new TEntity();
    //    //    //if (entity is IValidateDomain)
    //    //    //{
    //    //    //    var validatingObject = (IValidateDomain)entity;
    //    //    //    validatingObject.ValidateDomain();
    //    //    //    if (validatingObject.ValidationErrors.Any())
    //    //    //        throw new DomainValidationException(validatingObject.ValidationErrors);
    //    //    //}

    //    //    List<string> paramNames = GetParamNames(model);
    //    //    string cols = string.Join(",", paramNames);
    //    //    string colsParams = string.Join(",", paramNames.Select(p => "@" + p));
    //    //    var keyColumn = GetKeyProperty(entity);

    //    //    var builder = new StringBuilder();
    //    //    builder.Append("update ").Append(new TEntity().TableName).Append(" set ");
    //    //    builder.AppendLine(string.Join(",", paramNames.Where(e => e != nameof(EntityBase<TId>.Id)).Select(p => p + "= @" + p)));
    //    //    builder.Append($"where {keyColumn.Item1} = @Id");

    //    //    DynamicParameters parameters = new DynamicParameters(model);
    //    //    parameters.Add(nameof(EntityBase<TId>.Id), id);

    //    //    var sql = builder.ToString();
    //    //    using (var connection = new SqlConnection(new TConnectionString().ConnectionString))
    //    //    {
    //    //        connection.Open();
    //    //        var result = await connection.ExecuteAsync(sql, parameters);
    //    //        return result;
    //    //    }
    //    //}

    //    //public async Task<TDto> SelectAsync<TDto>(TId id, CancellationToken ct) where TDto : new()
    //    //{
    //    //    var entity = new TEntity();
    //    //    List<string> paramNames = GetParamNames(new TDto());
    //    //    paramNames.RemoveAll(e => _ignoreColumns.Contains(e));
    //    //    string cols = string.Join(",", paramNames);
    //    //    var keyColumn = GetKeyProperty(entity);

    //    //    using (var connection = new SqlConnection(new TConnectionString().ConnectionString))
    //    //    {
    //    //        connection.Open();
    //    //        return await connection.QueryFirstOrDefaultAsync<TDto>($"select {cols} from {entity.TableName} where {keyColumn.Item1} = @id", new { id });
    //    //    }
    //    //}

    //    //public async Task<IEnumerable<TDto>> SelectAsync<TDto>(Dictionary<string, object> whereAndModel = null,
    //    //                                                       Dictionary<string, object> whereOrModel = null,
    //    //                                                       CancellationToken ct = default) where TDto : new()
    //    //{
    //    //    var entity = new TEntity();
    //    //    List<string> paramNames = GetParamNames(new TDto());
    //    //    paramNames.RemoveAll(e => _ignoreColumns.Contains(e));
    //    //    string cols = string.Join(",", paramNames);

    //    //    StringBuilder whereClauseSB = new StringBuilder();
    //    //    DynamicParameters parameters = new DynamicParameters(whereAndModel);
    //    //    string whereClause = string.Empty;

    //    //    whereClauseSB.Append(" where ");
    //    //    if (whereAndModel != null)
    //    //    {
    //    //        foreach (var parameter in whereAndModel)
    //    //        {
    //    //            whereClauseSB.Append($"{parameter.Key}= @{parameter.Key} and ");
    //    //            parameters.Add(parameter.Key, parameter.Value);
    //    //        }
    //    //        whereClause += whereClauseSB.ToString().Trim().TrimEnd("and".ToArray());
    //    //    }

    //    //    if (whereOrModel != null)
    //    //    {
    //    //        whereClauseSB.Append(" and ");
    //    //        foreach (var parameter in whereAndModel)
    //    //        {
    //    //            whereClauseSB.Append($"{parameter.Key}= @{parameter.Key} or ");
    //    //            parameters.Add(parameter.Key, parameter.Value);
    //    //        }
    //    //        whereClause += whereClauseSB.ToString().Trim().TrimEnd("or".ToArray());
    //    //    }

    //    //    using (var connection = new SqlConnection(new TConnectionString().ConnectionString))
    //    //    {
    //    //        connection.Open();
    //    //        return await connection.QueryAsync<TDto>($"select {cols} from {entity.TableName} {whereClause}", parameters);
    //    //    }
    //    //}

    //    public async Task<IEnumerable<TDto>> ExecuteStoredProcedureAsync<TDto>(string spName, params object[] args)
    //    {


    //        var parametersSb = new StringBuilder();
    //        for (var i = 0; i < args.Length; i++)
    //            parametersSb.Append($" {{{i}}} ");

    //        using (var connection = new SqlConnection(new TConnectionString().ConnectionString))
    //        {
    //            connection.Open();
    //            return await connection.QueryAsync<TDto>($"Exec {spName};", parametersSb, commandType: System.Data.CommandType.StoredProcedure);
    //        }
    //    }

    //    private List<string> GetParamNames(object obj)
    //    {
    //        if (obj is DynamicParameters parameters)
    //            return parameters.ParameterNames.ToList();

    //        return _reflectionHelper.GetParamNames(obj);
    //    }

    //    private static Tuple<string, Type> GetKeyProperty(TEntity entity)
    //    {

    //        if (!_keyPropCache.TryGetValue(entity.GetType(), out var paramNames))
    //        {
    //            foreach (var prop in entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.GetGetMethod(false) != null))
    //            {
    //                var attribs = prop.GetCustomAttributes(typeof(KeyAttribute), true);
    //                var attr = attribs.FirstOrDefault() as KeyAttribute;
    //                if (attr != null)
    //                {
    //                    _keyPropCache.TryAdd(entity.GetType(), new Tuple<string, Type>(prop.Name, prop.PropertyType));
    //                    return new Tuple<string, Type>(prop.Name, prop.PropertyType);
    //                }
    //            }
    //            _keyPropCache[entity.GetType()] = paramNames;
    //        }
    //        return paramNames;
    //    }
    //}
}
