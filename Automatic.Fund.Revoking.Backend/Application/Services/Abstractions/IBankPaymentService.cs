using System;
using System.Collections.Generic;
using System.Linq;
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
    public interface IBankPaymentService : ICRUDService<BankPayment>
    {
        Task<PaginatedList<BankPaymentRs>> GetAll(int fundId, TransactionStatusEnum? status = null, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);

        Task<BankPaymentRs> GetById(int fundId, long id, CancellationToken cancellationToken = default);

        Task<PaginatedList<BankPaymentReportRs>> Report(int fundId, TransactionStatusEnum? status = null, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);

        Task<ExcelResultRs> Excel(int fundId, TransactionStatusEnum? status = null, CancellationToken cancellationToken = default);

    }
}
