using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests.ThirdParties.Pasargad;
using Application.Models.Responses.ThirdParties.Pasargad;
using Core.Abstractions;

namespace Application.Services.Abstractions.HttpClients.ThirdParties.BankingProviders
{
    public interface IPasargadBankClientService : IHttpClientService
    {
        Task<PasargadBaseRs<PasargadBankPayaPaymentRs>> PayaPayment(PasargadBankPayaPaymentRq request, CancellationToken cancellationToken = default);
        
        Task<PasargadBaseRs<PasargadBankInternalPaymentRs>> InternalPayment(PasargadBankInternalPaymentRq request, CancellationToken cancellationToken = default);
        
        Task<PasargadBaseRs<PasargadBankAccountBalanceRs>> GetAccountBalance(PasargadBankAccountBalanceRq request, CancellationToken cancellationToken = default);
        
        Task<PasargadBaseRs<PasargadBankSatnaPaymentRs>> SatnaPayment(PasargadBankSatnaPaymentRq request, CancellationToken cancellationToken = default);

        Task<PasargadBaseRs<PasargadBankKYCRs>> IsKYCCompliant(PasargadBankKYCRq request, CancellationToken cancellationToken = default);

    }
}
