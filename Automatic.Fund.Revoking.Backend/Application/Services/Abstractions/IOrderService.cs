using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Models.Responses.Reports;
using Core.Models;
using Core.Models.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Abstractions
{
    public interface IOrderService : ICRUDService<Order>
    {

        Task<PaginatedList<OrderRs>> GetAll(int fundId, OrderStatusEnum? status = null, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);

        Task<OrderRs> GetById(int fundId, long id, CancellationToken cancellationToken = default);

        IQueryable<Order> GetAllQueryable(int fundId, OrderStatusEnum? status = null);

        Task<Order> Create(int fundId, OrderCreateRq request, bool addToHistory = true, CancellationToken cancellationToken = default);

        Task<IEnumerable<Order>> CreateBatch(int fundId, IEnumerable<OrderCreateRq> request, bool addToHistory = true, CancellationToken cancellationToken = default);

        Task<Order> Update(int fundId, OrderUpdateRq request, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> UpdateBatch(int fundId, IEnumerable<Order> request, CancellationToken cancellationToken = default);

        Task<OrderRs> UpdateOrderStatus(int fundId, long orderId, OrderStatusEnum orderStatus, string orderStatusDescription, bool saveChanges = false, CancellationToken cancellationToken = default);

        Task<OrderRs> SetAsAccepted(int fundId, long orderId, string orderStatusDescription, CancellationToken cancellationToken = default);

        Task<OrderRs> SetAsRejected(int fundId, long orderId, string orderStatusDescription, CancellationToken cancellationToken = default);

        Task Delete(int fundId, long id, CancellationToken cancellationToken = default);

        Task<PaginatedList<OrderReportRs>> Report(int fundId, OrderStatusEnum? status = null, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);

        Task<ExcelResultRs> Excel(int fundId, OrderStatusEnum? status = null, CancellationToken cancellationToken = default);

        Task AddToHistory(Order order, CancellationToken cancellationToken = default);

    }
}
