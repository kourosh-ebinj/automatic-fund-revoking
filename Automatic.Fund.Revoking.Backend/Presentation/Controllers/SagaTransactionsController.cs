using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using System.Collections.Generic;
using Presentation.Attributes;
using Presentation.Filters;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace Presentation.Controllers
{
    [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
    public class SagaTransactionsController : BaseController<SagaTransactionsController>
    {
        private readonly ISagaTransactionService _sagaTransactionService;

        public SagaTransactionsController(ISagaTransactionService sagaTransactionService)
        {
            _sagaTransactionService = sagaTransactionService;
        }

        /// <summary>
        /// Retrieves all Saga Transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [SwaggerOperationFilter(typeof(FundIdParameterOperationFilter))]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<SagaTransactionRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _sagaTransactionService.GetAll(FundId);

            return GetOkResult(items);
        }


        /// <summary>
        /// Retrieves a Saga Transaction
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(OkResult<SagaTransactionRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var item = await _sagaTransactionService.GetById(id, cancellationToken);

            return GetOkResult(item);
        }

    }
}
