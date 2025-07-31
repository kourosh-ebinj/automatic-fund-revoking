using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Models.Responses;
using Application.Models.Responses.Reports;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Persistence;
using Core.Abstractions;
using Core.Constants;
using Core.Models;
using Core.Models.Responses;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{

    public class BankPaymentService : CRUDService<BankPayment>, IBankPaymentService
    {
        public IExcelBuilderService _excelBuilderService { get; set; }

        public BankPaymentService(IExcelBuilderService excelBuilderService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _excelBuilderService = excelBuilderService;
        }

        public async Task<PaginatedList<BankPaymentRs>> GetAll(int fundId, TransactionStatusEnum? status = null,
                                                               int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                               CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = $"createdat desc";

            var query = GetAllQueryable(fundId, status)
                         .Include(e => e.SourceBankAccount).ThenInclude(e => e.Bank).Include(e => e.DestinationBank);
            var list = await query.ToPaginatedList(pageSize ?? GlobalConstants.DefaultPageSize, pageNumber ?? 1, orderBy, cancellationToken);

            var orders = _mapper.Map<ICollection<BankPaymentRs>>(list.Items);

            return new PaginatedList<BankPaymentRs>()
            {
                PageNumber = list.PageNumber,
                Items = orders,
                PageSize = list.PageSize,
                TotalItems = list.TotalItems,
                TotalPages = list.TotalPages,
            };
        }

        public async Task<BankPaymentRs> GetById(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var order = await GetAllQueryable(fundId).FirstOrDefaultAsync(e => e.Id == id);
            var orderRs = _mapper.Map<BankPaymentRs>(order);

            return orderRs;
        }

        public IQueryable<BankPayment> GetAllQueryable(int fundId, TransactionStatusEnum? status = null)
        {
            var query = GetQuery().Include(e => e.SourceBankAccount)
                                  .Where(e => e.SourceBankAccount.FundId == fundId);
            if (status is not null)
                query = query.Where(e => e.TransactionStatusId == status);

            return query;
        }

        public async Task<PaginatedList<BankPaymentReportRs>> Report(int fundId, TransactionStatusEnum? status = null, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default)
        {
            var payments = await GetAll(fundId, status, pageSize, pageNumber, orderBy, cancellationToken);

            var items = payments.Items.Select(transaction => new BankPaymentReportRs()
            {
                Id = transaction.Id,
                BankPaymentMethodId = transaction.BankPaymentMethodId,
                BankUniqueId = transaction.BankUniqueId,
                Description = transaction.Description,
                DestinationBankId = transaction.DestinationBankId,
                DestinationBankName = transaction.DestinationBankName,
                DestinationShebaNumber = transaction.DestinationShebaNumber,
                OrderId = transaction.OrderId,
                SourceBankAccountNumber = transaction.SourceBankAccountNumber,
                SourceBankName = transaction.SourceBankName,
                TotalAmount = transaction.TotalAmount,
                TransactionStatusId = transaction.TransactionStatusId,

            }).ToImmutableList();

            return new PaginatedList<BankPaymentReportRs>
            {
                Items = items,
                PageNumber = payments.PageNumber,
                PageSize = payments.PageSize,
                TotalItems = payments.TotalItems,
                TotalPages = payments.TotalPages,
            };
        }

        public async Task<ExcelResultRs> Excel(int fundId, TransactionStatusEnum? status = null, CancellationToken cancellationToken = default)
        {
            var sheetName = "پرداخت های ثبت شده";

            var payments = await Report(fundId, status, pageSize: int.MaxValue, cancellationToken: cancellationToken);
            return new ExcelResultRs()
            {
                Bytes = await _excelBuilderService.ExportToExcel(payments.Items.ToBankPaymentReportExcelRs(), sheetName),
                Title = sheetName,
            };
        }

    }
}
