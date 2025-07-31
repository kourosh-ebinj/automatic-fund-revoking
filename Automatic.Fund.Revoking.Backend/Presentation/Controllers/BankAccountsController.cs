using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Models.Requests;
using Presentation.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Filters;

namespace Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/bank-accounts")] // Uncomment if you need to change the default routing
    [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
    public class BankAccountsController : BaseController<BankAccountsController>
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountsController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        // Get api has been implemented in fundscontroller

        ///// <summary>
        ///// Retrieves all bank accounts
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("")]
        //[ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(OkListResult<IEnumerable<BankAccountRs>>), StatusCodes.Status200OK)]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetAll(int fundId,  bool? isEnabled = null)
        //{
        //    var items = await _bankAccountService.GetAll(fundId, isEnabled);

        //    return GetOkResult(items);
        //}

        /// <summary>
        /// Creates a new bank account
        /// </summary>
        /// <param name="AccountNumber">the AccountNumber of the bank account to create</param>
        /// <param name="ShebaNumber" >the ShebaNumber of the bank account to create</param>
        /// <param name="BankId" >the BankId of the bank account to create</param>
        /// <param name="FundId" >the FundId of the bank account to create</param>
        /// <returns>BankAccountRs</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<BankAccountRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create(BankAccountCreateRq request, CancellationToken cancellationToken = default)
        {
            var result = await _bankAccountService.Create(FundId, request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Updates a bank account
        /// </summary>
        /// <param name="Id">the unique id of the bank account to update</param>
        /// <param name="Name">the unique name of the bank account to update</param>
        /// <param name="DsCode">the unique dscode of the bank account to update</param>
        /// <returns>BankAccountRs</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<BankAccountRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BankAccountUpdateRq request, CancellationToken cancellationToken = default)
        {
            request.Id = id;
            var result = await _bankAccountService.Update(FundId, request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a bank account
        /// </summary>
        /// <param name="FundId">the id of the bank account to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _bankAccountService.Delete(FundId, id, cancellationToken);

            return Ok();
        }

    }
}
