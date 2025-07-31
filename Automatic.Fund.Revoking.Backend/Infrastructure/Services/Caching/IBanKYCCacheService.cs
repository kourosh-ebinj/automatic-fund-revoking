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
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Caching
{
    public class IBanKYCCacheService : BaseHCqService<IBanKYC>, IIBanKYCCacheService
    {
        public IIBanKYCService _iBanKYCService { get; set; }

        public IBanKYCCacheService(ICacheProvider cacheProvider, IIBanKYCService iBanKYCService) : base(cacheProvider)
        {
            _iBanKYCService = iBanKYCService;

        }

        public override string CacheName => nameof(IBanKYC);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<Dictionary<string, IBanKYC>> GenerateAll(CacheKey key) =>
            await _iBanKYCService.GetAllQueryable().ToDictionaryAsync(e => e.IBan, x => x);

        public override async Task<IBanKYC> Generate(CacheKey key, CacheKey field)
        {
            var allItems = await GetAll("");
            if (!allItems.ContainsKey(field)) return null;

            return allItems[field];
        }

        public async Task<IEnumerable<IBanKYC>> GetAll(CancellationToken cancellationToken = default) =>
            (await GetAll("")).Values;

        public async Task<IBanKYC> GetByIBan(string iban, CancellationToken cancellationToken = default) =>
            await Get("", iban);

    }

}
