using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Models.Responses;

namespace Application.Services.Abstractions.ThirdParties.Banks
{
    public interface IBankingProviderService 
    {
        Task<double> GetAccountBalance(BankAccountRs bankAccount, CancellationToken cancellationToken = default);

        Task<BankPaymentMethodResultRs> InternalPayment(BankInternalPaymentRq request, CancellationToken cancellationToken = default);
        
        Task<BankPaymentMethodResultRs> PayaPayment(BankPayaPaymentRq request, CancellationToken cancellationToken = default);

        Task<BankPaymentMethodResultRs> SatnaPayment(BankSatnaPaymentRq request, CancellationToken cancellationToken = default);
    }
}
