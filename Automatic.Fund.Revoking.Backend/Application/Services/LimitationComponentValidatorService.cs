using System;
using System.Collections.Generic;
using Application.Models;
using Application.Services.Abstractions;
using Domain.Enums;

namespace Application.Services
{
    public class LimitationComponentValidatorService : ILimitationComponentValidatorService
    {

        public ILimitationComponentValidator GetLimitationComponentValidator(LimitationComponentTypeEnum limitationComponentType)
        {
            switch (limitationComponentType)
            {
                case LimitationComponentTypeEnum.BankAccountBalance:
                    return new BankAccountBalanceLimitationComponentValidator();
                case LimitationComponentTypeEnum.FundCancellationMinUnits:
                    return new CancellationMinUnitsLimitationComponentValidator();
                case LimitationComponentTypeEnum.FundCancellationMaxUnits:
                    return new CancellationMaxUnitsLimitationComponentValidator();
                case LimitationComponentTypeEnum.FundCancellationMinAmount:
                    return new CancellationMinAmountLimitationComponentValidator();
                case LimitationComponentTypeEnum.FundCancellationMaxAmount:
                    return new CancellationMaxAmountLimitationComponentValidator();
                case LimitationComponentTypeEnum.CustomerWhitelist:
                    return new CustomerWhitelistLimitationComponentValidator();
                case LimitationComponentTypeEnum.AppWhitelist:
                    return new AppWhitelistLimitationComponentValidator();
                default:
                    throw new InvalidOperationException("LimitationComponent not registered");
            }

        }

    }
}
