using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Services.Abstractions.ThirdParties.Banks;
using Domain.Enums;

namespace Application.Services.Abstractions
{
    public interface IPayMethodSelectorService
    {
        ValueTask<BankPaymentMethodEnum> GetAppropriatePaymentMethod(int sourceBankId, int destinationBankId, double amount, CancellationToken cancellationToken);

    }
}
