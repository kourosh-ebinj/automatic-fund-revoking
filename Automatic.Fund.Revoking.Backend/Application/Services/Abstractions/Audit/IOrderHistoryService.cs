using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Models.Responses;
using Core.Models;
using Domain.Entities.Audit;
using Domain.Enums;
using Application.Models.Responses;
using Application.Models.Responses.Reports;

namespace Application.Services.Abstractions.Audit
{
    public interface IOrderHistoryService
    {
        Task<OrderHistoryRs> GetById(int fundId, long id, CancellationToken cancellationToken = default);
        Task<PaginatedList<OrderHistoryRs>> GetAll(int fundId, long orderId, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);
        IQueryable<OrderHistory> GetAllQueryableByOrderId(int fundId, long orderId);
        IQueryable<OrderHistory> GetAllQueryable(int fundId);
        Task<OrderHistory> Create(OrderHistory entity, CancellationToken cancellationToken = default);

        Task<PaginatedList<OrderHistoryReportRs>> Report(int fundId, long orderId, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);
        Task<ExcelResultRs> Excel(int fundId, long orderId, CancellationToken cancellationToken = default);

    }
}
