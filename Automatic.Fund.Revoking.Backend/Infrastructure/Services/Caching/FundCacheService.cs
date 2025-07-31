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
    public class FundCacheService : BaseSCqService<IEnumerable<FundRs>>, IFundCacheService
    {
        public IFundService _fundService { get; set; }

        public FundCacheService(ICacheProvider cacheProvider, IFundService fundService) : base(cacheProvider)
        {
            _fundService = fundService;

        }

        public override string CacheName => nameof(FundRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<IEnumerable<FundRs>> Generate(CacheKey key)
        {
            var result = await _fundService.GetAllQueryable().ToListAsync();

            return _mapper.Map<IEnumerable<FundRs>>(result);
        }

        public async Task<IEnumerable<FundRs>> GetAll(bool? isEnabled = null)
        {
            var funds = await Get("");

            if (isEnabled is not null)
                funds = funds.Where(e => e.IsEnabled == isEnabled);

            return funds;
        }

        public async Task<FundRs> GetById(int id, CancellationToken cancellationToken = default)
        {
            var funds = await Get("");

            return funds.FirstOrDefault(e => e.Id == id);
        }

        public async Task<IEnumerable<FundRs>> GetActiveFunds(CancellationToken cancellationToken = default)
        {
            var banks = await GetAll();

            return banks.Where(e => e.IsEnabled);
        }

        public async Task<FundRs> GetActiveFundById(int bankId, CancellationToken cancellationToken = default)
        {
            var activeFunds= await GetActiveFunds(cancellationToken);

            return activeFunds.FirstOrDefault(e => e.Id == bankId);
        }


    }

}
