using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using System.Threading;
using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Filters;
using Presentation.Attributes;

namespace Presentation.Controllers
{
    [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
    public class CustomersController : BaseController<CustomersController>
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <param name="OrderStatusEnum">Optional, the status of the orders to retrieve</param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<CustomerRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(string keyword = "",
                                                int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                CancellationToken cancellationToken = default)
        {
            var items = await _customerService.GetAll(FundId, keyword, pageSize, pageNumber, orderBy, cancellationToken);

            return GetOkResult(items);
        }

        /// <summary>
        /// Retrieves customers
        /// </summary>
        /// <param name="customerId">required, Customer Id</param>
        /// <returns></returns>
        [HttpGet("GetByIds")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<CustomerRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetByIds([FromQuery] IEnumerable<long> customerIds, CancellationToken cancellationToken = default)
        {
            var items = await _customerService.GetByIds(FundId, customerIds, cancellationToken);

            return GetOkResult(items);
        }
    }
}
