using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Caching;
using Application.Services.Abstractions.Persistence;
using Core;
using Core.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{

    public class UserFundService : CRUDService<UserFund>, IUserFundService
    {
        private IUserFundCacheService _userFundCacheService => ServiceLocator.GetService<IUserFundCacheService>();

        public UserFundService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IEnumerable<UserFundRs>> GetAll(CancellationToken cancellationToken = default)
        {
            var userFunds = await GetAllQueryable().ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserFundRs>>(userFunds);
        }

        public async Task<IEnumerable<UserFundRs>> GetByUserId(long userId, CancellationToken cancellationToken = default) =>
            await _userFundCacheService.GetByUserId(userId, cancellationToken);

        public IQueryable<UserFund> GetAllQueryable(long? userId = null)
        {
            var query = GetQuery();

            if (userId is not null)
                query = query.Where(e => e.UserId == userId);

            return query;
        }

        public async Task<IEnumerable<FundRs>> GetUserFunds(long userId, IEnumerable<string> userRoles, CancellationToken cancellationToken = default)
        {
            _guard.Assert(userId > 0, ExceptionCodeEnum.BadRequest, "UserId is invalid.");
            _guard.Assert(userRoles.Any(), ExceptionCodeEnum.BadRequest, "UserRoles is invalid.");

            var fundService = ServiceLocator.GetService<IFundService>();
            var activeFunds = await fundService.GetActiveFunds();

            var systemAdmin = userRoles.FirstOrDefault(e => e.Equals(Constants.Role_SystemAdmin.ToString(), StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(systemAdmin))
                return activeFunds;

            var userFunds = await GetByUserId(userId, cancellationToken);
            var privilegedFunds = activeFunds.Where(e => userFunds.Select(e => e.FundId).Contains(e.Id));
            return privilegedFunds;
        }
        public async Task<UserFundRs> Create(UserFundCreateRq request, CancellationToken cancellationToken = default)
        {
            var userfund = await base.Create(new UserFund()
            {
                UserId = request.UserId,
                FundId = request.FundId,
            });
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<UserFundRs>(userfund);
        }

        public async Task<UserFundRs> Update(UserFundUpdateRq request, CancellationToken cancellationToken = default)
        {
            var userFund = await GetByIdInternal(request.Id, cancellationToken);
            _guard.Assert(userFund is not null, ExceptionCodeEnum.BadRequest, "ردیفی با این شناسه یافت نشد.");

            userFund.UserId = request.UserId;
            userFund.FundId = request.fundId;

            await base.Update(userFund);
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<UserFundRs>(userFund);
        }

        private async Task<UserFund> GetByIdInternal(int id, CancellationToken cancellationToken = default)
        {
            var fund = await GetAllQueryable().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return fund;
        }
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            var userFund = await GetByIdInternal(id, cancellationToken);
            _guard.Assert(userFund is not null, ExceptionCodeEnum.BadRequest, "ردیفی با این شناسه یافت نشد.");

            await base.Delete(userFund, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

    }
}

