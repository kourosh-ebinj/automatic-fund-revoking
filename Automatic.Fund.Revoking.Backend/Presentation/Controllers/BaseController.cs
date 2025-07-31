using System;
using System.Threading;
using Application.Models;
using Core;
using Core.Abstractions;
using Core.Constants;
using Presentation.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [SwaggerOperationFilter(typeof(CustomerIdParameterOperationFilter))]
    [SwaggerOperationFilter(typeof(RoleIdParameterOperationFilter))]
    [SwaggerOperationFilter(typeof(FundIdParameterOperationFilter))]
    public class BaseController<TController> : WebCore.Controllers.BaseController<TController, ApplicationSettingExtenderModel>
    {

        protected readonly IGuard _guard;

        protected int FundId => GetFundId();

        public BaseController()
        {
            _guard = ServiceLocator.GetService<IGuard>();

        }

        private int GetFundId() {

            if (Request.Headers.ContainsKey(GlobalConstants.FundIdKey)) 
                if(int.TryParse(Request.Headers[GlobalConstants.FundIdKey].ToString(), out int fundId)) 
                    return fundId;

            return 0;
        }
    }
}
