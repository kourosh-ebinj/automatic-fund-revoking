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
using Domain.Enums;
using Infrastructure.Services.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class LimitationComponentService : CRUDService<LimitationComponent>, ILimitationComponentService
    {
        private ILimitationComponentCacheService _limitationComponentCacheService => ServiceLocator.GetService<ILimitationComponentCacheService>();

        private readonly ILimitationComponentValidatorService _limitationComponentValidatorService;

        public LimitationComponentService(ILimitationComponentValidatorService limitationComponentValidatorService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _limitationComponentValidatorService = limitationComponentValidatorService;
        }

        public async Task<IEnumerable<LimitationComponentRs>> GetAll(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default)
        {
            var limitationComponents = await GetAllQueryable(fundId, isEnabled).ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<LimitationComponentRs>>(limitationComponents);
        }

        public async Task<LimitationComponentRs> GetById(int limitationId, int id, CancellationToken cancellationToken = default) =>
                await _limitationComponentCacheService.GetById(limitationId, id, cancellationToken);

        public async Task<IEnumerable<LimitationComponentRs>> GetByLimitationTypeId(int fundId, LimitationTypeEnum limitationTypeId, CancellationToken cancellationToken = default)
        {
            var limitationComponents = await GetAllQueryable(fundId)
                .Include(e => e.Limitation)
                .Where(x => x.Limitation.LimitationTypeId == limitationTypeId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<LimitationComponentRs>>(limitationComponents);
        }

        public async Task<IEnumerable<LimitationComponentRs>> GetByLimitationId(int fundId, int limitationId, CancellationToken cancellationToken = default)
        {
            var query = GetAllQueryable(fundId)
                .Where(e => e.LimitationId == limitationId);

            var limitationComponents = await query.ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<LimitationComponentRs>>(limitationComponents);
        }

        public async Task Update(LimitationComponentUpdateRq request, CancellationToken cancellationToken = default)
        {
            var limitationComponent = await GetQuery().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            _guard.Assert(limitationComponent is not null, ExceptionCodeEnum.BadRequest, "کامپوننتی با این آی دی یافت نشد.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.Value), ExceptionCodeEnum.BadRequest, "مقدار کامپوننت معتبر نمی باشد.");
            _guard.Assert(!string.IsNullOrWhiteSpace(request.Error), ExceptionCodeEnum.BadRequest, "متن خطای کامپوننت معتبر نمی باشد.");

            limitationComponent.Value = request.Value;
            limitationComponent.Error = request.Error;
            limitationComponent.Enabled = request.Enabled;
            await base.Update(limitationComponent);
            await _unitOfWork.SaveChanges(cancellationToken);

            await _limitationComponentCacheService.Remove(limitationComponent.LimitationId);
        }

        public async Task<object> GetLimitationComponentValue(int limitationId, int limitationComponentId, CancellationToken cancellationToken = default)
        {
            var limitationComponent = await GetById(limitationId, limitationComponentId, cancellationToken);
            _guard.Assert(limitationComponent is not null, ExceptionCodeEnum.BadRequest);

            var validator = _limitationComponentValidatorService.GetLimitationComponentValidator(limitationComponent.LimitationComponentTypeId);

            return validator.GetValueAsObject(limitationComponent.Value);
        }

        public IQueryable<LimitationComponent> GetAllQueryable(int fundId, bool? isEnabled = null)
        {
            var limitationsQueryable = GetQuery()
                .Include(e => e.Limitation)
                .Where(e => e.Limitation.FundId == fundId);

            if (isEnabled.HasValue)
                limitationsQueryable = limitationsQueryable.Where(e => e.Enabled == isEnabled.Value);

            return limitationsQueryable;
        }
    }
}
