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
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Caching
{
    public class LimitationCacheService : BaseHCqService<AllLimitationsRs>, ILimitationCacheService
    {
        public ILimitationService _limitationService { get; set; }

        public LimitationCacheService(ICacheProvider cacheProvider, ILimitationService limitationService) : base(cacheProvider)
        {
            _limitationService = limitationService;

        }

        public override string CacheName => nameof(LimitationRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<Dictionary<string, AllLimitationsRs>> GenerateAll(CacheKey key)
        {
            _guard.Assert(int.TryParse(key, out int fundId), ExceptionCodeEnum.BadRequest, "شناسه صندوق اشتباه است.");

            var limitations = await _limitationService.GetAllQueryable(fundId).
                                  ToDictionaryAsync(e => e.Id.ToString(), e => _mapper.Map<AllLimitationsRs>(e),
                                  default); //, cancellationToken

            return limitations;
        }

        public async override Task<AllLimitationsRs> Generate(CacheKey key, CacheKey field)
        {
            _guard.Assert(!int.TryParse(key, out int fundId), ExceptionCodeEnum.BadRequest, "شناسه صندوق اشتباه است.");
            _guard.Assert(!int.TryParse(field, out int limitationId), ExceptionCodeEnum.BadRequest, "شناسه محدودیت اشتباه است.");

            var limitation = await _limitationService.GetAllQueryable(fundId).
                                  FirstOrDefaultAsync(e => e.Id == limitationId,
                                  default); //, cancellationToken


            return _mapper.Map<AllLimitationsRs>(limitation);
        }

        public async Task<LimitationRs> GetLimitationByLimitationType(int fundId, LimitationTypeEnum limitationType)
        {
            var limitations = await GetAll(fundId);
            var limitation = limitations.Values.ToList().FirstOrDefault(e => e.LimitationTypeId == limitationType);
            
            if (limitation is not null)
                return _mapper.Map<LimitationRs>(limitation);

            return null;
        }

        public async Task<LimitationRs> GetById(int fundId, int id, CancellationToken cancellationToken = default)
        {
            var limitation = await Get(fundId, id);

            if (limitation is not null)
                return _mapper.Map<LimitationRs>(limitation);

            return null;
        }

    }

}
