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

    public class BankAccountService : CRUDService<BankAccount>, IBankAccountService
    {
        private IBankAccountCacheService _bankAccountCacheService => ServiceLocator.GetService<IBankAccountCacheService>();

        public BankAccountService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<BankAccountRs> GetById(int fundId, int id, CancellationToken cancellationToken = default) =>
            await _bankAccountCacheService.GetById(fundId, id, cancellationToken);


        public async Task<IEnumerable<BankAccountRs>> GetAll(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default)
        {
            var query = GetAllQueryable(fundId, isEnabled);
            var funds = await query.ToListAsync();

            return _mapper.Map<IEnumerable<BankAccountRs>>(funds);
        }

        public IQueryable<BankAccount> GetAllQueryable(int fundId, bool? isEnabled = null)
        {
            var query = GetQuery().Where(e => !e.IsDeleted).Where(e => e.FundId == fundId);

            if (isEnabled is not null)
                query = query.Where(e => e.IsEnabled == isEnabled);

            return query;
        }

        public async Task<IEnumerable<BankAccountRs>> GetBankAccountsByFundId(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default) =>
            await _bankAccountCacheService.GetAll(fundId, isEnabled, cancellationToken);

        public async Task<BankAccountRs> Create(int fundId, BankAccountCreateRq request, CancellationToken cancellationToken = default)
        {
            await ValidateToCreate(fundId, request);

            var entity = _mapper.Map<BankAccount>(request);
            entity.FundId = fundId;
            var bankAccount = await base.Create(entity);

            return _mapper.Map<BankAccountRs>(bankAccount);
        }

        public async Task<BankAccountRs> Update(int fundId, BankAccountUpdateRq request, CancellationToken cancellationToken = default)
        {
            var bankAccount = await GetByIdInternal(fundId, request.Id);
            _guard.Assert(bankAccount is not null, ExceptionCodeEnum.BadRequest, "صندوقی با این شناسه یافت نشد.");

            await ValidateToUpdate(fundId, request);

            bankAccount.AccountNumber = request.AccountNumber;
            bankAccount.ShebaNumber = request.ShebaNumber;
            bankAccount.BankId = request.BankId;
            bankAccount.FundId = fundId;

            await base.Update(bankAccount);
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<BankAccountRs>(bankAccount);
        }

        public async Task<BankAccountRs> UpdateBalance(int fundId, int id, double balance, CancellationToken cancellationToken = default)
        {
            var bankAccount = await GetByIdInternal(fundId, id);
            _guard.Assert(bankAccount is not null, ExceptionCodeEnum.BadRequest, "صندوقی با این شناسه یافت نشد.");

            bankAccount.Balance = balance;
            await base.Update(bankAccount);
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<BankAccountRs>(bankAccount);
        }

        public async Task Delete(int fundId, int id, CancellationToken cancellationToken = default)
        {
            var fund = await GetByIdInternal(fundId, id);
            _guard.Assert(fund is not null, ExceptionCodeEnum.BadRequest, "صندوقی با این شناسه یافت نشد.");

            await ValidateToDelete(fund);

            fund.IsDeleted = true;
            await base.Update(fund);
            await _unitOfWork.SaveChanges(cancellationToken);
        }

        private async Task<BankAccount> GetByIdInternal(int fundId, int id, CancellationToken cancellationToken = default)
        {
            //var bankAccount = await Find(id, cancellationToken);
            var bankAccount = await GetAllQueryable(fundId)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return bankAccount;
        }

        private async ValueTask ValidateToCreate(int fundId, BankAccountCreateRq request)
        {
            await ValidateBase(request.AccountNumber, request.ShebaNumber, request.BankId, fundId);

        }

        private async ValueTask ValidateToUpdate(int fundId, BankAccountUpdateRq request)
        {
            await ValidateBase(request.AccountNumber, request.ShebaNumber, request.BankId, fundId);

        }

        private async ValueTask ValidateToDelete(BankAccount entity)
        {

            await Task.CompletedTask;
        }

        private async ValueTask ValidateBase(string accountNumber, string shebaNumber, int bankId, int fundId)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(accountNumber), ExceptionCodeEnum.BadRequest, "مقدار شماره حساب نا معتبر است.");
            _guard.Assert(!string.IsNullOrWhiteSpace(shebaNumber), ExceptionCodeEnum.BadRequest, "مقدار شماره شبا نا معتبر است.");
            _guard.Assert(bankId > 0, ExceptionCodeEnum.BadRequest, "مقدار کد بانک نا معتبر است.");
            _guard.Assert(fundId > 0, ExceptionCodeEnum.BadRequest, "مقدار کد صندوق نا معتبر است.");

            await Task.CompletedTask;
        }

    }
}
