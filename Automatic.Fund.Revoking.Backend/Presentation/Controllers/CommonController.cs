using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebCore.Controllers;
using WebCore.Dtos;
using Application.Models;
using Microsoft.Extensions.Logging;
using Presentation.Models;
using Application.Services.Abstractions.Caching;
using Core.Extensions;
using System.Linq;
using Core.Constants;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    public class CommonController : BaseController<CustomersController, ApplicationSettingExtenderModel>
    {
        public IBankCacheService _bankCacheService { get; set; }

        public CommonController(IBankCacheService bankCacheService)
        {
            _bankCacheService = bankCacheService;

        }

        [AllowAnonymous]
        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <param name="OrderStatusEnum">Optional, the status of the orders to retrieve</param>
        /// <returns></returns>
        [HttpPost("log")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Log(LogModel request)
        {
            _logger.BeginScope("Frontend Log");
            var i = 1;
            Dictionary<string, object> parametersDic = new Dictionary<string, object>();
            request.Parameters ??= new List<string>();

            if (request.Parameters.Any())
                parametersDic = request.Parameters.ToDictionary(e => i++.ToString(), e => (object)e);

            if (string.IsNullOrWhiteSpace(request.Message))
                request.Message = "";
            if (string.IsNullOrWhiteSpace(request.Details))
                request.Details = "";

            parametersDic.Add("Message", request.Message);
            parametersDic.Add("Details", request.Details);

            if (!string.IsNullOrWhiteSpace(request.CorrelationId))
                parametersDic.Add(GlobalConstants.CorrelationIdKey, request.CorrelationId);

            _logger.Log(request.LogType, parametersDic);

            //switch (request.LogType)
            //{
            //    case LogLevel.Trace:
            //        _logger.LogTrace(request.Message, request.Parameters);
            //        break;
            //    case LogLevel.Debug:
            //        _logger.LogDebug(request.Message, request.Parameters);
            //        break;
            //    case LogLevel.Information:
            //        _logger.LogInformation(request.Message, request.Parameters);
            //        break;
            //    case LogLevel.Warning:
            //        _logger.LogWarning(request.Message, request.Parameters);
            //        break;
            //    case LogLevel.Error:
            //    case LogLevel.Critical:
            //        _logger.Log(request.LogType, parametersDic);
            //        break;
            //}

            return Ok();
        }


        [HttpPost("Test")]
        [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> TestAsync()
        {

            return Ok();
        }

    }
}
