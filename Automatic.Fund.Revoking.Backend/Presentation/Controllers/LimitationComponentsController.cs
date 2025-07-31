using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using System.Threading;
using Application.Models.Requests;
using Presentation.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Filters;
using Domain.Enums;

namespace Presentation.Controllers
{
    [Route("api/v{version:apiVersion}/limitation-components")] // Uncomment if you need to change the default routing
    public class LimitationComponentsController : BaseController<LimitationComponentsController>
    {
        private readonly ILimitationComponentService _limitationComponentService;

        public LimitationComponentsController(ILimitationComponentService limitationComponentService)
        {
            _limitationComponentService = limitationComponentService;

        }

        /// <summary>
        /// Retrieves limitationComponents by limitationId
        /// </summary>
        /// <param name="limitationId">the id of the limitation entity</param>
        /// <returns></returns>
        [HttpGet("limitationtype/{limitationTypeId}")]
        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<LimitationComponentRs>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetByLimitationId(LimitationTypeEnum limitationTypeId, CancellationToken cancellationToken)
        {
            var items = await _limitationComponentService.GetByLimitationTypeId(FundId, limitationTypeId, cancellationToken);

            return GetOkResult(items);
        }

        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin)]
        /// <summary>
        /// Retrieves limitationComponents by limitationId
        /// </summary>
        /// <param name="limitationId">the id of the limitation entity</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(int id, [FromBody] LimitationComponentUpdateRq request, CancellationToken cancellationToken)
        {
            request.Id = id;
            await _limitationComponentService.Update(request, cancellationToken);

            return Ok();
        }
    }
}
