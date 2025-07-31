using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace WebCore.Extensions
{
    public static class HttpRequestExtensions
    {
        public static int? GetFundId(this HttpRequest request)
        {

            if (request.Headers.ContainsKey(GlobalConstants.FundIdKey))
                if (int.TryParse(request.Headers[GlobalConstants.FundIdKey].ToString(), out var fundId))
                    return fundId;

            return null;
        }

        public static long GetUserId(this HttpRequest request)
        {
            var customerIdParam = request.Headers[GlobalConstants.CustomerIdKey].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(customerIdParam))
                return 0;

            if (!long.TryParse(customerIdParam, out var customerId))
            {
                var ids = customerIdParam.ToString().Split(',');
                customerId = Convert.ToInt64(ids.First());
            }
            return customerId;
        }

        public static IEnumerable<string> GetUserRoles(this HttpRequest request)
        {
            var roles = request.Headers[GlobalConstants.RoleIdKey];
            if (string.IsNullOrWhiteSpace(roles))
                return Enumerable.Empty<string>();

            return roles.ToString().FromJsonString<IEnumerable<string>>();
        }
    }
}
