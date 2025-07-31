using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enums;
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
    public class LimitationService : CRUDService<Limitation>, ILimitationService
    {
        private ILimitationCacheService _limitationCacheService => ServiceLocator.GetService<ILimitationCacheService>();
        private readonly ILimitationComponentValidatorService _limitationComponentValidatorService;

        public LimitationService(ILimitationComponentValidatorService limitationComponentValidatorService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _limitationComponentValidatorService = limitationComponentValidatorService;
        }

        public IEnumerable<ILimitationComponentValidator> LimitationComponents { get; set; }
        public ILimitationComponentService LimitationComponentService { get; set; }

        public async Task<IEnumerable<AllLimitationsRs>> GetAll(int fundId)
        {
            var limitations = await _limitationCacheService.GetAll(fundId);

            return _mapper.Map<IEnumerable<AllLimitationsRs>>(limitations.Values);
        }

        public IQueryable<Limitation> GetAllQueryable(int fundId)
        {
            var limitationsQueryable = GetQuery()
                                    .Include(x => x.LimitationComponents)
                                    .Where(e => e.FundId == fundId);

            return limitationsQueryable;
        }

        public async Task<LimitationRs> GetById(int fundId, int id) =>
            await _limitationCacheService.GetById(fundId, id);

        //public async Task<Limitation> Create(LimitationRq limitationRq)
        //{
        //    var title = limitationRq.Title?.Trim();
        //    var error = limitationRq.Error?.Trim();

        //    GuardAgainstTitle(title);
        //    GuardAgainstError(error);

        //    var dateLimitationComponent = limitationRq.LimitationComponents.FirstOrDefault(x => x.Type == LimitationComponentTypeEnum.DateLimitation);

        //    if (dateLimitationComponent != null)
        //    {
        //        var value = DateLimitationComponent.CastToValidType(dateLimitationComponent.Value);
        //        Guard.Assert(value.Max >= value.Min,
        //        , "تاریخ شروع باید کوچکتر از تاریخ پایان باشد");
        //    }

        //    var limitation = new Limitation
        //    {
        //        Title = title,
        //        Error = error,
        //        Enabled = false
        //    };

        //    await CreateChild(limitation, limitationRq.LimitationComponents);

        //    await Create(limitation);
        //    await UnitOfWork.SaveChanges();
        //    await UpdateCache(limitation);

        //    return limitation;
        //}

        //public async Task<Limitation> Update(LimitationRq limitationRq)
        //{

        //    var title = limitationRq.Title?.Trim();
        //    var error = limitationRq.Error?.Trim();

        //    GuardAgainstTitle(title);
        //    GuardAgainstError(error);

        //    var limitation = await GetById(limitationRq.Id);

        //    limitation.Title = title;
        //    limitation.Error = error;

        //    await Update(limitation);

        //    await LimitationComponentService.Delete(limitation.LimitationComponents);
        //    await LimitationPrincipalService.Delete(limitation.LimitationPrincipals);

        //    await CreateChild(limitation, limitationRq.LimitationComponents);

        //    await UnitOfWork.SaveChanges();

        //    await UpdateCache(limitation);

        //    return limitation;
        //}

        //public async Task Delete(int id)
        //{
        //    var limitation = await GetById(id);

        //    await Delete(limitation);

        //    await LimitationComponentService.Delete(limitation.LimitationComponents);
        //    await LimitationPrincipalService.Delete(limitation.LimitationPrincipals);

        //    await UnitOfWork.SaveChanges();

        //    await UpdateCache(limitation);
        //}

        //private async Task UpdateCache(Limitation limitation)
        //{
        //    foreach (var limitationPrincipal in limitation.LimitationPrincipals)
        //    {
        //        await GetLimitations(limitationPrincipal.PrincipalId, false);
        //    }
        //}


        //private async Task CreateChild(Limitation limitation, List<LimitationComponentRs> limitationComponents)
        //{
        //    var principalLimitationComponents = new List<LimitationComponentRs>();
        //    foreach (var limitationComponent in limitationComponents)
        //    {
        //        if (PrincipalLimitationsTypes.Contains(limitationComponent.Type) && !limitationComponent.Exclude)
        //            principalLimitationComponents.Add(limitationComponent);

        //        var limitationComponentService = LimitationComponents.First(x => x.Type == limitationComponent.Type);
        //        limitation.LimitationComponents.Add(await limitationComponentService.Generate(limitationComponent));

        //    }

        //    if (principalLimitationComponents.Any())
        //    {
        //        var limitationPrincipals = principalLimitationComponents
        //            .SelectMany(x => JsonConvert.DeserializeObject<List<long>>(JsonConvert.SerializeObject(x.Value))).Distinct()
        //            .Select(x => new LimitationPrincipal
        //            {
        //                PrincipalId = x
        //            }).ToList();

        //        limitationPrincipals.ForEach(x => limitation.LimitationPrincipals.Add(x));
        //    }
        //    else
        //    {
        //        limitation.LimitationPrincipals.Add(new LimitationPrincipal
        //        {
        //            PrincipalId = 0
        //        });
        //    }
        //}

        public async Task<LimitationRs> GetLimitationByLimitationType(int fundId, LimitationTypeEnum limitationType)
        {
            var limitations = await _limitationCacheService.GetLimitationByLimitationType(fundId, limitationType);

            return limitations;
        }

        public async Task<LimitationComponentValidatorResultRs> Validate(LimitationTypeEnum limitationType, int fundId, Order order)
        {
            var limitation = await GetLimitationIncludingComponents(fundId, limitationType);

            var tasks = limitation.LimitationComponents.Where(e => e.Enabled).Select(async limitationComponent =>
            {
                var limitationComponentValidator = _limitationComponentValidatorService.GetLimitationComponentValidator(limitationComponent.LimitationComponentTypeId);
                return await limitationComponentValidator.Validate(limitation, limitationComponent, fundId, order);
            });
            var results = await Task.WhenAll(tasks);

            var rejectedItem = results.Where(e => e.ValidatorResultStatus == LimitationComponentValidatorResultEnum.Rejected).FirstOrDefault();
            var needApprovalItem = results.Where(e => e.ValidatorResultStatus == LimitationComponentValidatorResultEnum.NeedsApproval).FirstOrDefault();
            if (rejectedItem is not null)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = rejectedItem.ValidatorResultStatus,
                    StatusMessage = rejectedItem.StatusMessage,
                };
            else if (needApprovalItem is not null)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = needApprovalItem.ValidatorResultStatus,
                    StatusMessage = needApprovalItem.StatusMessage,
                };
            else
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted,
                    StatusMessage = string.Empty,
                };
        }

        private void GuardAgainstError(string error)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(error), ExceptionCodeEnum.BadRequest, "متن خطا اجباری است");
            _guard.Assert(error.Length <= 1000, ExceptionCodeEnum.BadRequest, "خطا حداکثر 1000 کاراکتر باید باشد");
        }

        private void GuardAgainstTitle(string title)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(title), ExceptionCodeEnum.BadRequest, "عنوان اجباری است");
            _guard.Assert(title.Length <= 200, ExceptionCodeEnum.BadRequest, "عنوان حداکثر 200 کاراکتر باید باشد");
        }

        private async Task<Limitation> GetLimitationIncludingComponents(int fundId, LimitationTypeEnum limitationTypeId)
        {
            var query = GetAllQueryable(fundId);
            var limitation = await query.FirstOrDefaultAsync(e => e.LimitationTypeId == limitationTypeId);

            return limitation;

        }


    }
}
