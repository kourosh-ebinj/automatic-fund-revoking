using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Models.Requests;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Filters;
using Presentation.Attributes;

namespace Presentation.Controllers
{
    public class BanksController : BaseController<BanksController>
    {
        private readonly IBankService _bankService;

        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
        }

        /// <summary>
        /// Retrieves all banks
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<FundRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(bool? isEnabled = null)
        {
            var items = await _bankService.GetAll(FundId, isEnabled);

            return GetOkResult(items);
        }

        /// <summary>
        /// Updates a bank
        /// </summary>
        /// <param name="Id">the unique id of the fund to update</param>
        /// <param name="Name">the unique name of the fund to update</param>
        /// <param name="DsCode">the unique dscode of the fund to update</param>
        /// <returns>FundRs</returns>
        [HttpPut("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<BankRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] BankUpdateRq request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _bankService.Update(FundId, request, cancellationToken);

            return GetOkResult(result);
        }

    }
}
