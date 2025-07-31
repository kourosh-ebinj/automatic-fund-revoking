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
using Infrastructure.Services.Caching;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{
    public class IBanKYCService : CRUDService<IBanKYC>, IIBanKYCService
    {
        private IIBanKYCCacheService _iBanKYCCacheService => ServiceLocator.GetService<IIBanKYCCacheService>();

        public IBanKYCService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IBanKYC> GetById(int id, CancellationToken cancellationToken = default) =>
            await GetByIdInternal(id, cancellationToken);

        public async Task<IBanKYC> GetByIBan(string iban, CancellationToken cancellationToken = default) =>
            await _iBanKYCCacheService.GetByIBan(iban, cancellationToken);

        public async Task<IEnumerable<IBanKYC>> GetAll(CancellationToken cancellationToken = default) =>
            await _iBanKYCCacheService.GetAll(cancellationToken);

        public IQueryable<IBanKYC> GetAllQueryable()
        {
            var query = GetQuery();

            return query;
        }

        public async Task<IBanKYC> Create(string iBanKYC, bool isKYCCompliant, CancellationToken cancellationToken = default)
        {
            await ValidateToCreate(iBanKYC, isKYCCompliant);

            var entity = new IBanKYC()
            {
                IBan = iBanKYC,
                IsKYCCompliant = isKYCCompliant,
            };
            entity = await base.Create(entity);
            await _unitOfWork.SaveChanges(cancellationToken);
            await _iBanKYCCacheService.Remove("");

            return entity;
        }

        private async Task<IBanKYC> GetByIdInternal(int id, CancellationToken cancellationToken = default) =>
            await GetAllQueryable().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        private async ValueTask ValidateToCreate(string iBanKYC, bool isKYCVerified)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(iBanKYC), ExceptionCodeEnum.BadRequest, "مقدار شماره شبا نا معتبر است.");

        }

    }
}

