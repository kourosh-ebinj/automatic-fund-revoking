using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Abstractions.ThirdParties.Banks
{
    public interface IBankingProviderSuggestService
    {
        Task<(BankAccount SourceBankAccount, BankPaymentMethodEnum BankPaymentMethod)> GetBestSuggestion(
                int fundId, 
                long orderId, 
                double orderTotalAmount, 
                int customerAccountBankId, 
                CancellationToken cancellationToken);

    }
}
