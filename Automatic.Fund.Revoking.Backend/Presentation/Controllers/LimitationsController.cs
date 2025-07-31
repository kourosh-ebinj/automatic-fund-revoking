using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCore.Dtos;
using Application.Models.Responses;
using Application.Services.Abstractions;
using System.Collections.Generic;
using Application.Models.Requests;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Filters;
using Presentation.Attributes;

namespace Presentation.Controllers
{
    public class LimitationsController : BaseController<LimitationsController>
    {
        private readonly ILimitationService _limitationService;

        public LimitationsController(ILimitationService limitationService)
        {
            _limitationService = limitationService;

        }

        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        /// <summary>
        /// Retrieves all limitations
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkListResult<IEnumerable<AllLimitationsRs>>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _limitationService.GetAll(FundId);

            return GetOkResult(items);
        }

        [UserFundRoleAuthorization(Application.Constants.Role_SystemAdmin, Application.Constants.Role_FundsAdmin, Application.Constants.Role_FundManager, Application.Constants.Role_Custodian)]
        /// <summary>
        /// Retrieves limitation by Id
        /// </summary>
        /// <param name="limitationId">the id of the limitation entity</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(OkResult<LimitationComponentUpdateRq>), StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(int id)
        {
            var items = await _limitationService.GetById(FundId, id);

            return GetOkResult(items);
        }

        //[HttpPut("")]
        //[ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> Update(int limitationId, string value)
        //{
        //    await _limitationService.Update(limitationId, value);

        //    return Ok();
        //}
    }
}
