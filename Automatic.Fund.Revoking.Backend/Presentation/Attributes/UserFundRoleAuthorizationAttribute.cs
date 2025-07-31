using System;
using System.Collections.Generic;
using System.Linq;
using Application.Services.Abstractions;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebCore.Dtos;
using WebCore.Extensions;

namespace Presentation.Attributes
{
    public class UserFundRoleAuthorizationAttribute : Attribute, IAuthorizationFilter //AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnumerable<string> _allowedRoles;

        public UserFundRoleAuthorizationAttribute(params string[] allowedRoles)
        {
            _httpContextAccessor = ServiceLocator.GetService<IHttpContextAccessor>();
            _allowedRoles = allowedRoles;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            long userId = context.HttpContext.Request.GetUserId();
            if (userId < 1)
            {
                context.Result = new BadRequestObjectResult(GetErrorObject("شناسه کاربر اشتباه است."));
                return;
            }

            var userRoles = context.HttpContext.Request.GetUserRoles();
            if (!userRoles.Any())
            {
                context.Result = new BadRequestObjectResult(GetErrorObject("کاربر دسترسی ندارد."));
                return;
            }

            var userHasRole = userRoles.Any(e => _allowedRoles.Contains(e));
            if (!userHasRole)
            {
                context.Result = new BadRequestObjectResult(GetErrorObject("دسترسی غیرمجاز"));
                return;
            }

            var userFundService = ServiceLocator.GetService<IUserFundService>();

            var fundId = context.HttpContext.Request.GetFundId();
            var userFunds = await userFundService.GetUserFunds(userId, userRoles);

            if (fundId.HasValue)
            {
                if (!userFunds.Select(e => e.Id).Contains(fundId.Value))
                {
                    context.Result = new BadRequestObjectResult(GetErrorObject("دسترسی غیرمجاز"));
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult(GetErrorObject("شناسه صندوق یافت نشد."));
                return;

            }
        }

        private NotOkResultDto GetErrorObject(params string[] parameters)
        {
            var correlationId = _httpContextAccessor.HttpContext.GetCorrelationId();
            var error = new NotOkResultDto(
                "دسترسی غیرمجاز", 
                CorrelationId: correlationId, 
                Parameters: parameters);
            
            return error;
        }
    }
}
