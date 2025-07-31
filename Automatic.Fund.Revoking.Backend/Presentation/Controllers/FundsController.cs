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
using Presentation.Attributes;
using WebCore.Extensions;

namespace Presentation.Controllers
{
    public class FundsController : BaseController<FundsController>
    {
        private readonly IFundService _fundService;
        private readonly IBankAccountService _bankAccountService;
        private readonly ILimitationService _limitationService;

        public FundsController(IFundService fundService, IBankAccountService bankAccountService, ILimitationService limitationService)
        {
            _fundService = fundService;
            _bankAccountService = bankAccountService;
            _limitationService = limitationService;
        }

        /// <summary>
        /// Retrieves all funds
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<FundRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(bool? isEnabled = null)
        {
            var items = await _fundService.GetAll(Request.GetUserId(), Request.GetUserRoles(), isEnabled);

            return GetOkResult(items);
        }

        /// <summary>
        /// Retrieves bank accounts by fundId
        /// </summary>
        /// <param name="fundId">the id of the fund entity</param>
        /// <returns></returns>   
        [HttpGet("{fundId}/bank-accounts")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<BankAccountRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetByFundId(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default)
        {
            var items = await _bankAccountService.GetBankAccountsByFundId(fundId, isEnabled, cancellationToken);

            return GetOkResult(items);
        }


        /// <summary>
        /// Retrieves all limitations
        /// </summary>
        /// <returns></returns>
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [HttpGet("{fundId}/limitations")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<AllLimitationsRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllLimitations(int fundId)
        {
            var items = await _limitationService.GetAll(fundId);

            return GetOkResult(items);
        }

        /// <summary>
        /// Retrieves limitation by Id
        /// </summary>
        /// <param name="limitationId">the id of the limitation entity</param>
        /// <returns></returns>
        [HttpGet("{fundId}/limitation/{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<LimitationComponentUpdateRq>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(int fundId, int id)
        {
            var items = await _limitationService.GetById(fundId, id);

            return GetOkResult(items);
        }

        /// <summary>
        /// Creates a new fund
        /// </summary>
        /// <param name="Name">the unique name of the fund to create</param>
        /// <param name="DsCode">the unique dscode of the fund to create</param>
        /// <returns>FundRs</returns>
        [HttpPost("")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<FundRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromBody] FundCreateRq request, CancellationToken cancellationToken = default)
        {
            var result = await _fundService.Create(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates a fund
        /// </summary>
        /// <param name="Id">the unique id of the fund to update</param>
        /// <param name="Name">the unique name of the fund to update</param>
        /// <param name="DsCode">the unique dscode of the fund to update</param>
        /// <returns>FundRs</returns>
        [HttpPut("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<FundRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] FundUpdateRq request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _fundService.Update(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a fund
        /// </summary>
        /// <param name="FundId">the id of the fund to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _fundService.Delete(id, cancellationToken);

            return Ok();
        }

    }
}
