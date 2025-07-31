using System;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Presentation.Scheduler
{
    public class HangFireDashboardLANAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var ipAddress = httpContext.Connection.RemoteIpAddress;
            var v4IpAddress = ipAddress.MapToIPv4();
            var host = httpContext.Request.Host.ToString();

            //Console.WriteLine($"HangFire Authorization Filter: Host {host} , Ipaddress {v4IpAddress} ");

            if (host.StartsWith("localhost", StringComparison.InvariantCultureIgnoreCase) ||
                host.StartsWith("127.0.0.1", StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (v4IpAddress.ToString().StartsWith("192.168.1."))
                return true;
            if (v4IpAddress.ToString().StartsWith("10.233."))
                return true;

            return false;
        }
    }
}
