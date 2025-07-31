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

    public class BankService : CRUDService<Bank>, IBankService
    {
        private IBankCacheService _bankCacheService => ServiceLocator.GetService<IBankCacheService>();

        public BankService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IEnumerable<BankRs>> GetAll(int fundId, bool? isEnabled = null) =>
            await _bankCacheService.GetAll(fundId, isEnabled);

        public IQueryable<Bank> GetAllQueryable(int fundId, bool? isEnabled = null)
        {
            var query = GetQuery().Where(e => e.FundId == fundId);

            if (isEnabled is not null)
                query = query.Where(e => e.IsEnabled == isEnabled);

            return query;
        }

        public async Task<IEnumerable<BankRs>> GetActiveBanks(int fundId, CancellationToken cancellationToken = default) =>
            await _bankCacheService.GetActiveBanks(fundId, cancellationToken);

        public async Task<BankRs> GetActiveBankById(int fundId, int bankId, CancellationToken cancellationToken = default) =>
            await _bankCacheService.GetActiveBankById(fundId, bankId, cancellationToken);

        public async Task<BankRs> GetById(int fundId, int id, CancellationToken cancellationToken = default) =>
            await _bankCacheService.GetById(fundId, id, cancellationToken);

        public async Task<BankRs> Update(int fundId, BankUpdateRq request, CancellationToken cancellationToken = default)
        {
            var bank = await GetByIdInternal(fundId, request.Id);
            _guard.Assert(bank is not null, ExceptionCodeEnum.BadRequest, "بانکی با این شناسه یافت نشد.");

            await ValidateToUpdate(fundId, request);

            bank.Name = request.Name;
            bank.Description = request.Description;
            bank.IsEnabled = request.IsEnabled;
            bank.FundId = fundId;

            await base.Update(bank);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _bankCacheService.ClearAll();
            return _mapper.Map<BankRs>(bank);
        }

        private async Task<Bank> GetByIdInternal(int fundId, int id, CancellationToken cancellationToken = default)
        {
            var bank = await GetAllQueryable(fundId).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return bank;
        }

        private async ValueTask ValidateToUpdate(int fundId, BankUpdateRq request)
        {
            _guard.Assert(fundId > 0, ExceptionCodeEnum.BadRequest, "شناسه صندوق نا معتبر است.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.Name), ExceptionCodeEnum.BadRequest, "نام بانک نا معتبر است.");

            await Task.CompletedTask;
        }

    }
}
