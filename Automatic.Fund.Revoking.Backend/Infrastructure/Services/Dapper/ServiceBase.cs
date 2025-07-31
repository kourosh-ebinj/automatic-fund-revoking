using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Abstractions.Persistence.Providers.Dapper;

namespace Infrastructure.Services.Dapper
{
    

    //public abstract class ServiceBase<TEntity, TConnectionString> : IServiceBase<TEntity, TConnectionString>
    //                                                                     where TEntity : EntityBase, new()
    //                                                                     where TConnectionString : IConnectionString, new()
    //{
    //    protected readonly IRepository<TEntity, TConnectionString> _repository;
    //    protected readonly IMapper _mapper;
    //    protected readonly ApplicationSettingModel _applicationSetting;

    //    public ServiceBase(
    //           IRepository<TEntity, TConnectionString> repository,
    //           IMapper mapper,
    //           ApplicationSettingModel applicationSetting)
    //    {
    //        _repository = repository;
    //        _mapper = mapper;
    //        _applicationSetting = applicationSetting;
    //    }

    //    //public async Task<TDto> SelectAsync<TDto>(TId id, CancellationToken ct) where TDto : new() =>
    //    //       await _repository.SelectAsync<TDto>(id, ct);

    //    //public async Task<IEnumerable<TDto>> SelectAsync<TDto>(Dictionary<string, object> whereAndModel = null, Dictionary<string, object> whereOrModel = null, CancellationToken ct = default) where TDto : new() =>
    //    //       await _repository.SelectAsync<TDto>(whereAndModel, whereOrModel, ct);

    //    //public async Task<int> InsertAsync(TEntity model, CancellationToken ct) =>
    //    //       await _repository.InsertAsync(model, ct);

    //    //public async Task<int> UpdateAsync(TId id, object model, CancellationToken ct) =>
    //    //       await _repository.UpdateAsync(id, model, ct);

    //    //public async Task<int> DeleteAsync(TId id, CancellationToken ct) =>
    //    //       await _repository.DeleteAsync(id, ct);

    //}
}
