using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models.Responses.ThirdParties.Rayan;
using Core.Abstractions;

namespace Application.Services.Abstractions.HttpClients.ThirdParties
{
    public interface IRayanClientService : IHttpClientService
    {
        public const int PAGESIZE = 10000;

        Task<IEnumerable<RayanFundOrderRs>> GetFundRequests(int dsCode, DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = PAGESIZE, CancellationToken cancellationToken = default);
        Task<RayanListBase<RayanCustomerRs>> GetCustomersList(int dsCode, int pageNumber = 1, int pageSize = PAGESIZE, CancellationToken cancellationToken = default);
        Task<RayanCustomerInfoRs> GetCustomerInfo(int dsCode, string nationalCode, CancellationToken cancellationToken = default);
        Task<bool> UpdateOrderStatus(int dsCode, long orderId, string orderDate, RayanStatusEnum fromStatus, RayanStatusEnum toStatus, CancellationToken cancellationToken = default);
    }
}
