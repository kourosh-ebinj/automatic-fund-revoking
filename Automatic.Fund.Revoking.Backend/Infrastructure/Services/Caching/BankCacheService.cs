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
    public class BankCacheService : BaseSCqService<IEnumerable<BankRs>>, IBankCacheService
    {
        public IBankService _bankService { get; set; }

        public BankCacheService(ICacheProvider cacheProvider, IBankService bankService) : base(cacheProvider)
        {
            _bankService = bankService;

        }

        public override string CacheName => nameof(BankRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<IEnumerable<BankRs>> Generate(CacheKey key)
        {
            _guard.Assert(int.TryParse(key, out int fundId), ExceptionCodeEnum.BadRequest, "شناسه صندوق اشتباه است.");

            var result = await _bankService.GetAllQueryable(fundId).ToListAsync();

            return _mapper.Map<IEnumerable<BankRs>>(result);
        }

        public async Task<IEnumerable<BankRs>> GetAll(int fundId, bool? isEnabled = null)
        {
            var banks = await Get(fundId);

            if (isEnabled is not null)
                banks = banks.Where(e => e.IsEnabled);

            return banks;
        }

        public async Task<BankRs> GetById(int fundId, int id, CancellationToken cancellationToken = default)
        {
            var banks = await GetAll(fundId);

            return banks.FirstOrDefault(e => e.Id == id);
        }

        public async Task<IEnumerable<BankRs>> GetActiveBanks(int fundId, CancellationToken cancellationToken = default)
        {
            var activeBanks = await GetAll(fundId, true);

            return activeBanks;
        }

        public async Task<BankRs> GetActiveBankById(int fundId, int bankId, CancellationToken cancellationToken = default)
        {
            var activeBanks = await GetActiveBanks(fundId, cancellationToken);

            return activeBanks.FirstOrDefault(e => e.Id == bankId);
        }

    }

}
