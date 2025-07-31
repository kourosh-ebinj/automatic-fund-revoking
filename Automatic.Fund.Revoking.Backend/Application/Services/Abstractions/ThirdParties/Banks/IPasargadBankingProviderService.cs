using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests.ThirdParties.Pasargad;
using Application.Models.Responses.ThirdParties.Pasargad;

namespace Application.Services.Abstractions.ThirdParties.Banks
{
    public interface IPasargadBankingProviderService : IBankingProviderService
    {
        Task<PasargadBaseRs<PasargadBankAccountBalanceRs>> GetAccountBalance(PasargadBankAccountBalanceRq request, CancellationToken cancellationToken = default);

        Task<PasargadBaseRs<PasargadBankInternalPaymentRs>> InternalPayment(PasargadBankInternalPaymentRq request, CancellationToken cancellationToken = default);

        Task<PasargadBaseRs<PasargadBankPayaPaymentRs>> PayaPayment(PasargadBankPayaPaymentRq request, CancellationToken cancellationToken = default);

        Task<PasargadBaseRs<PasargadBankSatnaPaymentRs>> SatnaPayment(PasargadBankSatnaPaymentRq request, CancellationToken cancellationToken = default);

        Task<PasargadBaseRs<PasargadBankKYCRs>> IsKYCCompliant(PasargadBankKYCRq request, CancellationToken cancellationToken = default);

    }
}
