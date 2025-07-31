using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties.Pasargad;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Caching;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core.Abstractions.Caching;
using Core.Enums;
using Core.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Caching
{
    public class PasargadBankAccountDetailCacheService : BaseHCqService<PasargadBankAccountDetailRs>, IPasargadBankAccountDetailCacheService
    {
        public IPasargadBankAccountDetailService _pasargadBankAccountDetailService { get; set; }

        public PasargadBankAccountDetailCacheService(ICacheProvider cacheProvider, IPasargadBankAccountDetailService pasargadBankAccountDetailService)
            : base(cacheProvider)
        {
            _pasargadBankAccountDetailService = pasargadBankAccountDetailService;

        }

        public override string CacheName => nameof(PasargadBankAccountDetailRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<Dictionary<string, PasargadBankAccountDetailRs>> GenerateAll(CacheKey key) =>
            await _pasargadBankAccountDetailService.GetAllQueryable()
                        .ToDictionaryAsync(e => e.BankAccountId.ToString(),
                                                x => _mapper.Map<PasargadBankAccountDetailRs>(x));

        public override async Task<PasargadBankAccountDetailRs> Generate(CacheKey key, CacheKey field)
        {
            var allItems = await GetAll("");
            if (!allItems.ContainsKey(field)) return null;

            return allItems[field];
        }

        public async Task<IEnumerable<PasargadBankAccountDetailRs>> GetAll(CancellationToken cancellationToken = default) =>
            (await GetAll("")).Values;

        public async Task<PasargadBankAccountDetailRs> GetByBankAccountId(int bankAccountId, CancellationToken cancellationToken = default) =>
            await Get("", bankAccountId);

    }

}
