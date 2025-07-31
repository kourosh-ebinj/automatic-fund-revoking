using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses;
using Application.Models.Responses.ThirdParties.Rayan;
using Domain.Entities.ThirdParties;

namespace Application.Services.Abstractions.ThirdParties
{
    public interface IRayanService
    {
        Task<IEnumerable<CancelledFundRs>> GetCancelledFunds(int dsCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<CustomerInfoRs> GetCustomerInfo(int dsCode, string nationalCode, CancellationToken cancellationToken = default);
        Task<PaginatedRayanCustomerRs> SyncRayanCustomers(FundRs fund, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<bool> MarkOrderAsConfirmed(int dsCode, long rayanFundOrderId, CancellationToken cancellationToken = default);
        Task<bool> ReverseOrderToDraft(int dsCode, long rayanFundOrderId, CancellationToken cancellationToken = default);

    }
}
