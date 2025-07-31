using Hangfire.Dashboard;

namespace Presentation.Scheduler
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //var httpContext = context.GetHttpContext();

            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}