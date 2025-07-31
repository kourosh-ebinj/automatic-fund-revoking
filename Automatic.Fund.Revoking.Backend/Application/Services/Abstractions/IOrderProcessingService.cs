using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Abstractions
{
    public interface IOrderProcessingService
    {
        Task<IEnumerable<Order>> ImportRayanCancelledFundsToOrders(int fundId, DateTime startDate, DateTime endDate, CancellationToken cancellationtoken = default);

        Task<IEnumerable<Order>> PayAcceptedOrders(int fundId, CancellationToken cancellationtoken = default);

    }
}
