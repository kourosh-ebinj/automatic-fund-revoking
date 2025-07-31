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
using System.Collections.Generic;
using Application.Services.Abstractions.Audit;
using Core.Models.Responses;
using Presentation.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Filters;

namespace Presentation.Controllers
{
    public class OrdersController : BaseController<OrdersController>
    {
        private readonly IOrderService _orderService;
        private readonly IOrderHistoryService _orderHistoryService;
        private readonly ISagaTransactionService _sagaTransactionService;

        public OrdersController(IOrderService orderService, IOrderHistoryService orderHistoryService, ISagaTransactionService sagaTransactionService)
        {
            _orderService = orderService;
            _orderHistoryService = orderHistoryService;
            _sagaTransactionService = sagaTransactionService;
        }

        /// <summary>
        /// Retrieves all orders
        /// </summary>
        /// <param name="OrderStatusEnum">Optional, the status of the orders to retrieve</param>
        /// <returns></returns>
        [HttpGet("")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<OrderRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(OrderStatusEnum? status = null,
                                                int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                CancellationToken cancellationToken = default)
        {
            var items = await _orderService.GetAll(FundId, status, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(items);
        }

        [HttpGet("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<OrderRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var item = await _orderService.GetById(FundId, id, cancellationToken);

            return GetOkResult(item);
        }

        [HttpPut("{id}/accept")]
        [UserFundRoleAuthorization(Application.Constants.Role_FundManager)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<OrderRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SetAsAccepted(long id, string orderStatusDescription,
                                                       CancellationToken cancellationToken = default)
        {
            var result = await _orderService.SetAsAccepted(FundId, id, orderStatusDescription, cancellationToken);

            return GetOkResult(result);
        }

        [HttpPut("{id}/reject")]
        [UserFundRoleAuthorization(Application.Constants.Role_FundManager)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<OrderRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SetAsRejected(long id, string orderStatusDescription,
                                                       CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(orderStatusDescription))
                orderStatusDescription = "سفارش توسط مدیر ابطال شد.";
            var result = await _orderService.SetAsRejected(FundId, id, orderStatusDescription, cancellationToken);

            return GetOkResult(result);
        }

        [HttpGet("report")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<OrderReportRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Report(OrderStatusEnum? status = null,
                                               int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                               CancellationToken cancellationToken = default)
        {
            var orders = await _orderService.Report(FundId, status, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(orders);
        }

        [HttpGet("excel")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<ExcelFileResult>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<FileContentResult> Excel(OrderStatusEnum? status = null, CancellationToken cancellationToken = default)
        {
            var result = await _orderService.Excel(FundId, status, cancellationToken);

            return new ExcelFileResult($"{result.Title}-{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", result.Bytes);
        }

        /// <summary>
        /// Retrieves all order histories
        /// </summary>
        /// <returns></returns>
        [HttpGet("{orderId}/histories")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<FundRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> OrderHistoryGetAll([FromRoute] long orderId, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default)
        {
            var items = await _orderHistoryService.GetAll(FundId, orderId, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(items);
        }

        /// <summary>
        /// Retrieves all order histories report
        /// </summary>
        /// <returns></returns>
        [HttpGet("{orderId}/histories/report")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<OrderHistoryRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> OrderHistoryReport([FromRoute] long orderId, int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default)
        {
            var items = await _orderHistoryService.Report(FundId, orderId, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(items);
        }

        /// <summary>
        /// Exports the excel file of all the changes applied to a particular order 
        /// </summary>
        /// <param name="orderId">the id of the order</param>
        /// <returns></returns>
        [HttpGet("{orderId}/histories/excel")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<ExcelResultRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<FileContentResult> OrderHistoryExcel([FromRoute] long orderId, CancellationToken cancellationToken = default)
        {
            var result = await _orderHistoryService.Excel(FundId, orderId, cancellationToken);

            return new ExcelFileResult($"{result.Title}-{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", result.Bytes);
        }


        /// <summary>
        /// Deletes an order softly (isDeleted = true)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<OrderRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken = default)
        {
            await _orderService.Delete(FundId, id, cancellationToken);

            return Ok();
        }

        [HttpGet("{id}/transaction")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(OkResult<SagaTransactionRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSagaTransaction(long id, CancellationToken cancellationToken = default)
        {
            var transaction = await _sagaTransactionService.GetByOrderIdOrDefault(id, cancellationToken);

            return GetOkResult(transaction);
        }

    }
}
