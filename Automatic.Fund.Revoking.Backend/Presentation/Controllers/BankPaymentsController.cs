using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Domain.Enums;
using System.Threading;
using System;
using WebCore.MVC.ContentResult;
using Application.Models.Responses.Reports;
using Presentation.Attributes;

namespace Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/bank-payments")] // Uncomment if you need to change the default routing
    [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
    public class BankPaymentsController : BaseController<OrdersController>
    {
        private readonly IBankPaymentService _bankPaymentService;

        public BankPaymentsController(IBankPaymentService bankPaymentService)
        {
            _bankPaymentService = bankPaymentService;
        }

        /// <summary>
        /// Retrieves all payments
        /// </summary>
        /// <param name="OrderStatusEnum">Optional, the status of the orders to retrieve</param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<BankPaymentRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(TransactionStatusEnum? status = null,
                                                int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                CancellationToken cancellationToken = default)
        {
            var items = await _bankPaymentService.GetAll(FundId, status, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<BankPaymentRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var item = await _bankPaymentService.GetById(FundId, id, cancellationToken);

            return GetOkResult(item);
        }

        [HttpGet("Report")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<BankPaymentReportRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Report(TransactionStatusEnum? status = null,
                                                int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                CancellationToken cancellationToken = default)
        {
            var orders = await _bankPaymentService.Report(FundId, status, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(orders);
        }

        [HttpGet("Excel")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<ExcelFileResult>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<FileContentResult> Excel(TransactionStatusEnum? status = null, CancellationToken cancellationToken = default)
        {
            var result = await _bankPaymentService.Excel(FundId, status, cancellationToken);

            return new ExcelFileResult($"{result.Title}-{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", result.Bytes);
        }

    }
}
