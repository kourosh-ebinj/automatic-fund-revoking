using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Caching;
using Core.Abstractions.Caching;
using Core.Enums;
using Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Caching
{
    public class LimitationComponentCacheService : BaseHCqService<LimitationComponentRs>, ILimitationComponentCacheService
    {
        public ILimitationService _limitationService { get; set; }
        public ILimitationComponentService _limitationComponentService { get; set; }

        public LimitationComponentCacheService(ICacheProvider cacheProvider,
                    ILimitationComponentService limitationComponentService,
                    ILimitationService limitationService) : base(cacheProvider)
        {
            _limitationComponentService = limitationComponentService;
            _limitationService = limitationService;
        }

        public override string CacheName => nameof(LimitationComponentRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<Dictionary<string, LimitationComponentRs>> GenerateAll(CacheKey key)
        {
            _guard.Assert(int.TryParse(key, out var limitationId), ExceptionCodeEnum.BadRequest, "شناسه محدودیت اشتباه است.");

            var limitation = await _limitationService.GetQuery().Select(e => new { e.Id, e.FundId }).FirstOrDefaultAsync(e => e.Id == limitationId);
            _guard.Assert(limitation is not null, ExceptionCodeEnum.BadRequest, "شناسه محدودیت اشتباه است.");

            var limitationComponents = await _limitationComponentService.GetAllQueryable(limitation.FundId).
                                  Where(e => e.LimitationId == limitationId).
                                  ToDictionaryAsync(e => e.Id.ToString(), e => _mapper.Map<LimitationComponentRs>(e),
                                  default); //, cancellationToken

            return limitationComponents;
        }

        public async override Task<LimitationComponentRs> Generate(CacheKey key, CacheKey field)
        {
            _guard.Assert(!int.TryParse(key, out int limitationId), ExceptionCodeEnum.BadRequest, "شناسه محدودیت اشتباه است.");
            _guard.Assert(!int.TryParse(field, out int limitationComponentId), ExceptionCodeEnum.BadRequest, "شناسه کامپوننت محدودیت اشتباه است.");

            var limitation = await _limitationService.GetQuery().Select(e => new { e.Id, e.FundId }).FirstOrDefaultAsync(e => e.Id == limitationId);
            _guard.Assert(limitation is not null, ExceptionCodeEnum.BadRequest, "شناسه محدودیت اشتباه است.");

            var limitationComponent = await _limitationComponentService.GetAllQueryable(limitation.FundId).
                                  FirstOrDefaultAsync(e => e.LimitationId == limitationId && e.Id == limitationComponentId,
                                  default); //, cancellationToken

            return _mapper.Map<LimitationComponentRs>(limitationComponent);
        }

        public async Task<IEnumerable<LimitationComponentRs>> GetAll(int limitationId, bool? isEnabled = null, CancellationToken cancellationToken = default)
        {
            var limitationComponents = await GetAll(limitationId, useCache: true);

            return limitationComponents.Values.ToList();
        }

        public async Task<LimitationComponentRs> GetById(int limitationId, int id, CancellationToken cancellationToken = default)
        {
            var bankAccount = await Get(limitationId, id, true);

            return bankAccount;
        }

    }
}
