using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebCore.Helpers
{
    public class APIResultHelper
    {
        public static object RestResultBody(string message, IEnumerable<string> parameters)
        {
            return ResultBody(message: message, parameters: parameters);
        }

        public static object RestResultBody(string message, string details = default)
        {
            return ResultBody(message: message, details: details);
        }

        public static object RestResultBody(string message, string correlationId, string details, IEnumerable<string> parameters)
        {
            return ResultBody(message: message, details: details, correlationId: correlationId, parameters: parameters);
        }

        public static string StringifyParameter(string key, object value) =>
                       $"{key}: {value}";

        public static string CreateErrorMessage(string url, HttpStatusCode httpStatusCode) =>
            $"An error occurred in api call with url: {url}. HttpStatusCode: {httpStatusCode}";

        /// <summary>
        /// Use this method for returning a common message signature in Apis
        /// </summary>
        /// <seealso cref="ControllerBase"/>
        /// <seealso>
        ///     <cref>PaymentGatewayMicro.Infrastructure.Dtos.HttpStatusMessageDto</cref>
        /// </seealso>
        /// <param name="message">message</param>
        /// <param name="correlationId"></param>
        /// <param name="parameters">list of parameters (e.g. the invalid parameter)</param>
        /// <param name="details"></param>
        /// <returns></returns>
        private static object ResultBody(string? message = default, string? details = default, string? correlationId = default, IEnumerable<string> parameters = default)
        {
            return new Dtos.NotOkResultDto(Message: message, Details: details, CorrelationId: correlationId, parameters);
        }
    }
}
