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
    public class UserFundCacheService : BaseSCqService<IEnumerable<UserFundRs>>, IUserFundCacheService
    {
        public IUserFundService _userFundService { get; set; }

        public UserFundCacheService(ICacheProvider cacheProvider, IUserFundService userFundService) : base(cacheProvider)
        {
            _userFundService = userFundService;

        }

        public override string CacheName => nameof(UserFundRs);
        public override bool HasSaveBehavior => true;
        //public override DateTimeOffset? Expiration => DateTimeOffset.Now.AddMinutes(1);

        protected override string CreateUniqueKeyName(CacheKey key)
        {
            //key = "*";

            return base.CreateUniqueKeyName(key);
        }

        public override async Task<IEnumerable<UserFundRs>> Generate(CacheKey key)
        {
            _guard.Assert(long.TryParse(key, out long userId), ExceptionCodeEnum.BadRequest, "شناسه کاربر اشتباه است.");

            var result = await _userFundService.GetAllQueryable(userId).ToListAsync();

            return _mapper.Map<IEnumerable<UserFundRs>>(result);
        }

        
        public async Task<IEnumerable<UserFundRs>> GetByUserId(long userId, CancellationToken cancellationToken = default)
        {
            var userFunds = await Get(userId);

            return userFunds;
        }

        public async Task<UserFundRs> GetById(long userId, int id, CancellationToken cancellationToken = default)
        {
            var userFunds = await GetByUserId(userId, cancellationToken);

            return userFunds.FirstOrDefault(e => e.Id == id);
        }
    }

}
