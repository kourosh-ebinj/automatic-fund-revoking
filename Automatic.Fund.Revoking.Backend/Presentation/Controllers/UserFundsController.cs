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
using Presentation.Filters;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Attributes;
using WebCore.Extensions;

namespace Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/user-funds")] // Uncomment if you need to change the default routing
    //[UserFundRoleAuthorization(Constants.Role_SystemAdmin, Constants.Role_FundsAdmin, Constants.Role_FundManager)]
    public class UserFundsController : BaseController<UserFundsController>
    {
        private readonly IUserFundService _userFundService;

        public UserFundsController(IUserFundService userFundService)
        {
            _userFundService = userFundService;
        }

        /// <summary>
        /// Get current user' funds
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<FundRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserFunds()
        {
            var funds = await _userFundService.GetUserFunds(Request.GetUserId(), Request.GetUserRoles());

            return Ok(funds);
        }

        /// <summary>
        /// Retrieves all userfunds
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<FundRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var items = await _userFundService.GetAll(cancellationToken);

            return GetOkResult(items);
        }

        /// <summary>
        /// Creates a new userfund
        /// </summary>
        /// <param name="Name">the unique name of the fund to create</param>
        /// <param name="DsCode">the unique dscode of the fund to create</param>
        /// <returns>FundRs</returns>
        [HttpPost("")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<FundRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromBody] UserFundCreateRq request, CancellationToken cancellationToken = default)
        {
            var result = await _userFundService.Create(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates a fund
        /// </summary>
        /// <param name="Id">the unique id of the fund to update</param>
        /// <param name="UserId">the id of the user </param>
        /// <param name="FundId">the fund assigned to the user</param>
        /// <returns>FundRs</returns>
        [HttpPut("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<FundRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserFundUpdateRq request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _userFundService.Update(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a userfund
        /// </summary>
        /// <param name="UserFundId">the id of the userfund to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _userFundService.Delete(id, cancellationToken);

            return Ok();
        }

    }
}
