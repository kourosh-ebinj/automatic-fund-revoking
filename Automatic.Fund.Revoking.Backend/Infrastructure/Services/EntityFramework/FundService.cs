using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class FundService : CRUDService<Fund>, IFundService
    {
        private readonly IUserFundService _userFundService;
        private IFundCacheService _fundCacheService => ServiceLocator.GetService<IFundCacheService>();

        public FundService(IUnitOfWork unitOfWork, IUserFundService userFundService) : base(unitOfWork)
        {
            _userFundService = userFundService;
        }

        public async Task<FundRs> GetById(int id, CancellationToken cancellationToken = default) =>
            await _fundCacheService.GetById(id, cancellationToken);

        public async Task<IEnumerable<FundRs>> GetAll(long userId, IEnumerable<string> userRoles, bool? isEnabled = null)
        {
            var allFunds = await _fundCacheService.GetAll(isEnabled);

            var userFunds = await _userFundService.GetUserFunds(userId, userRoles);

            return allFunds.Intersect(userFunds);
        }

        public IQueryable<Fund> GetAllQueryable(bool? isEnabled = null)
        {
            var query = GetQuery().Where(e => !e.IsDeleted);

            if (isEnabled is not null)
                query = query.Where(e => e.IsEnabled == isEnabled);

            return query;
        }

        public async Task<FundRs> Create(FundCreateRq request, CancellationToken cancellationToken = default)
        {
            await ValidateToCreate(request);

            var entity = _mapper.Map<Fund>(request);
            var fund = await base.Create(entity);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _fundCacheService.ClearAll();

            return _mapper.Map<FundRs>(fund);
        }

        public async Task<FundRs> Update(FundUpdateRq request, CancellationToken cancellationToken = default)
        {
            var fund = await GetByIdInternal(request.Id);
            _guard.Assert(fund is not null, ExceptionCodeEnum.BadRequest, "صندوقی با این شناسه یافت نشد.");

            await ValidateToUpdate(request);

            fund.Name = request.Name;
            fund.DsCode = request.DsCode;
            fund.IsEnabled = request.IsEnabled;

            await base.Update(fund);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _fundCacheService.ClearAll();

            return _mapper.Map<FundRs>(fund);
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            var fund = await GetByIdInternal(id);
            _guard.Assert(fund is not null, ExceptionCodeEnum.BadRequest, "صندوقی با این شناسه یافت نشد.");

            await ValidateToDelete(fund);

            fund.IsDeleted = true;
            await base.Update(fund);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _fundCacheService.ClearAll();
        }

        public async Task<IEnumerable<FundRs>> GetActiveFunds(CancellationToken cancellationToken = default) =>
            await _fundCacheService.GetActiveFunds(cancellationToken);

        public async Task<FundRs> GetActiveFundById(int fundId, CancellationToken cancellationToken = default) =>
            await _fundCacheService.GetActiveFundById(fundId, cancellationToken);

        private async Task<Fund> GetByIdInternal(int id, CancellationToken cancellationToken = default)
        {
            var fund = await GetAllQueryable().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return fund;
        }

        private async ValueTask ValidateToCreate(FundCreateRq request)
        {
            await ValidateBase(request.Name, request.DsCode);

            var fundWithDuplicateName = await GetAllQueryable().Where(e => e.Name == request.Name).FirstOrDefaultAsync();
            _guard.Assert(fundWithDuplicateName is null, ExceptionCodeEnum.BadRequest, "مقدار نام صندوق تکراری است.");

            var fundWithDuplicateDsCode = await GetAllQueryable().Where(e => e.DsCode == request.DsCode).FirstOrDefaultAsync();
            _guard.Assert(fundWithDuplicateDsCode is null, ExceptionCodeEnum.BadRequest, "مقدار کد صندوق تکراری است.");

        }

        private async ValueTask ValidateToUpdate(FundUpdateRq request)
        {
            await ValidateBase(request.Name, request.DsCode);

            var fundWithDuplicateName = await GetAllQueryable().Where(e => e.Name == request.Name && e.Id != request.Id).FirstOrDefaultAsync();
            _guard.Assert(fundWithDuplicateName is null, ExceptionCodeEnum.BadRequest, "مقدار نام صندوق تکراری است.");

            var fundWithDuplicateDsCode = await GetAllQueryable().Where(e => e.DsCode == request.DsCode && e.Id != request.Id).FirstOrDefaultAsync();
            _guard.Assert(fundWithDuplicateDsCode is null, ExceptionCodeEnum.BadRequest, "مقدار کد صندوق تکراری است.");

        }

        private async ValueTask ValidateToDelete(Fund entity)
        {

            await Task.CompletedTask;
        }

        private async ValueTask ValidateBase(string name, int dsCode)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(name), ExceptionCodeEnum.BadRequest, "مقدار نام صندوق نا معتبر است.");
            _guard.Assert(dsCode > 1, ExceptionCodeEnum.BadRequest, "مقدار کد صندوق نا معتبر است.");

            await Task.CompletedTask;
        }

    }
}

