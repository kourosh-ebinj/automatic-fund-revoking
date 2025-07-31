using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.HttpClients;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core;
using Core.Extensions;
using Core.Services;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;

namespace Application.Services
{
    public abstract class LimitationComponentValidatorBase : ILimitationComponentValidator
    {
        public abstract LimitationComponentTypeEnum Type { get; }

        public abstract Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default);

        public abstract object GetValueAsObject(string jsonValue);

        /// <summary>
        /// T is the type of component value in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected static T CastToValidType<T>(string limitationComponentValue)
        {
            return limitationComponentValue.FromJsonString<T>();
        }

        //public virtual Task<LimitationComponent> Generate(LimitationComponent limitationComponent)
        //{
        //    CastToValidType(limitationComponent.Value);

        //    return Task.FromResult(new LimitationComponent
        //    {
        //        LimitationComponentTypeId = Type,
        //        Value = limitationComponent.Value,
        //        Exclude = limitationComponent.Exclude
        //    });
        //}

    }

    #region Pre Ordering

    public class CancellationMinUnitsLimitationComponentValidator : LimitationComponentValidatorBase
    {
        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.FundCancellationMinUnits;

        public override object GetValueAsObject(string jsonValue) => CastToValidType<int>(jsonValue);

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId,Order order, CancellationToken cancellationToken = default)
        {
            var minUnitsToCancelInOneTransaction = CastToValidType<int>(limitationComponent.Value);

            if (order.TotalUnits < minUnitsToCancelInOneTransaction)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.Rejected,
                    StatusMessage = limitationComponent.Error,
                };

            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    public class CancellationMaxUnitsLimitationComponentValidator : LimitationComponentValidatorBase
    {
        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.FundCancellationMaxUnits;

        public override object GetValueAsObject(string jsonValue) => CastToValidType<int>(jsonValue);

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default)
        {
            var maxUnitsToCancelInOneTransaction = CastToValidType<int>(limitationComponent.Value);

            if (order.TotalUnits > maxUnitsToCancelInOneTransaction)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.NeedsApproval,
                    StatusMessage = limitationComponent.Error,
                };

            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    public class CancellationMinAmountLimitationComponentValidator : LimitationComponentValidatorBase
    {
        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.FundCancellationMinAmount;

        public override object GetValueAsObject(string jsonValue) => CastToValidType<long>(jsonValue);

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default)
        {
            var minAmountToCancelInOneTransaction = CastToValidType<long>(limitationComponent.Value);

            if (order.TotalAmount < minAmountToCancelInOneTransaction)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.Rejected,
                    StatusMessage = limitationComponent.Error,
                };
            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    public class CancellationMaxAmountLimitationComponentValidator : LimitationComponentValidatorBase
    {
        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.FundCancellationMaxAmount;

        public override object GetValueAsObject(string jsonValue) => CastToValidType<long>(jsonValue);

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default)
        {
            var maxAmountToCancelInOneTransaction = CastToValidType<long>(limitationComponent.Value);

            if (order.TotalAmount > maxAmountToCancelInOneTransaction)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.NeedsApproval,
                    StatusMessage = limitationComponent.Error,
                };

            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    public class CustomerWhitelistLimitationComponentValidator : LimitationComponentValidatorBase
    {
        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.CustomerWhitelist;

        public override object GetValueAsObject(string jsonValue) => CastToValidType<long>(jsonValue);

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default)
        {
            var customerIds = CastToValidType<IEnumerable<long>>(limitationComponent.Value);

            if (!customerIds.Contains(order.CustomerId))
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.Rejected,
                    StatusMessage = limitationComponent.Error,
                };

            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    public class AppWhitelistLimitationComponentValidator : LimitationComponentValidatorBase
    {
        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.AppWhitelist;

        public override object GetValueAsObject(string jsonValue) => CastToValidType<string>(jsonValue);

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default)
        {
            var platforms = CastToValidType<IEnumerable<string>>(limitationComponent.Value);

            if (!platforms.Contains(order.AppCode))
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.Rejected,
                    StatusMessage = limitationComponent.Error,
                };

            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    #endregion

    #region PrePayment

    public class BankAccountBalanceLimitationComponentValidator : LimitationComponentValidatorBase
    {
        private readonly ICachingService _cachingService;
        private readonly IBankingProviderSuggestService _bankingProviderSuggestService;

        public BankAccountBalanceLimitationComponentValidator()
        {
            _cachingService = ServiceLocator.GetService<ICachingService>();
            _bankingProviderSuggestService = ServiceLocator.GetService<IBankingProviderSuggestService>();

        }

        public override object GetValueAsObject(string jsonValue) => CastToValidType<BankAccountLimitationInputRs>(jsonValue);

        public override LimitationComponentTypeEnum Type => LimitationComponentTypeEnum.BankAccountBalance;

        public async override Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default)
        {
            var paymentSuggestionModel = await _bankingProviderSuggestService.GetBestSuggestion(fundId, order.Id, order.TotalAmount, order.CustomerAccountBankId, cancellationToken);

            var inputModel = CastToValidType<BankAccountLimitationInputRs>(limitationComponent.Value);

            if (paymentSuggestionModel.SourceBankAccount.Balance < inputModel.MinBalance)
                return new LimitationComponentValidatorResultRs()
                {
                    ValidatorResultStatus = LimitationComponentValidatorResultEnum.Rejected,
                    StatusMessage = limitationComponent.Error,
                };

            return new LimitationComponentValidatorResultRs()
            {
                ValidatorResultStatus = LimitationComponentValidatorResultEnum.Accepted
            };
        }
    }

    #endregion
}
