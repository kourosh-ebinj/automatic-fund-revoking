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
    public class BankAccountCacheService : BaseHCqService<BankAccountRs>, IBankAccountCacheService
    {
        public IBankAccountService _bankAccountService { get; set; }

        public BankAccountCacheService(ICacheProvider cacheProvider, IBankAccountService bankAccountService) : base(cacheProvider)
        {
            _bankAccountService = bankAccountService;

        }

        public override string CacheName => nameof(BankAccountRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<Dictionary<string, BankAccountRs>> GenerateAll(CacheKey key)
        {
            _guard.Assert(int.TryParse(key, out int fundId), ExceptionCodeEnum.BadRequest, "شناسه صندوق اشتباه است.");

            var bankAccounts = await _bankAccountService.GetAllQueryable(fundId).
                                  ToDictionaryAsync(e => e.Id.ToString(), e => _mapper.Map<BankAccountRs>(e),
                                  default); //, cancellationToken

            return bankAccounts;
        }

        public async override Task<BankAccountRs> Generate(CacheKey key, CacheKey field)
        {
            _guard.Assert(!int.TryParse(key, out int fundId), ExceptionCodeEnum.BadRequest, "شناسه صندوق اشتباه است.");
            _guard.Assert(!int.TryParse(field, out int bankAccountId), ExceptionCodeEnum.BadRequest, "شناسه شماره حساب اشتباه است.");

            var bankAccount = await _bankAccountService.GetAllQueryable(fundId).
                                  FirstOrDefaultAsync(e => e.Id == bankAccountId,
                                  default); //, cancellationToken

            return _mapper.Map<BankAccountRs>(bankAccount);
        }

        public async Task<IEnumerable<BankAccountRs>> GetAll(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default)
        {
            var bankAccounts = await GetAll(fundId, useCache: true);

            return bankAccounts.Values.ToList();
        }

        public async Task<BankAccountRs> GetById(int fundId, int id, CancellationToken cancellationToken = default)
        {
            var bankAccount = await Get(fundId, id, true);

            return bankAccount;
        }


    }
}
