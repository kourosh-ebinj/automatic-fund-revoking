using System;

namespace Core.Constants
{
    public sealed class GlobalConstants
    {
        public const string PostmanMockResponseKey = "x-mock-response-name";
        public const string HealthCheckRoute = "/hc";
        public const string HealthCheckLiveRoute = HealthCheckRoute + "/live";
        public const string HealthCheckReadyRoute = HealthCheckRoute + "/ready";
        public const string HealthCheckVersionRoute = HealthCheckRoute + "/version";
        public const int DefaultPageSize = 25;
        public const string CorrelationIdKey = "CorrelationId";
        public const string PriceSqlDataType = "decimal(15,2)";

        public const string CustomerIdKey = "customer-id";
        public const string RoleIdKey = "Roles";
        public const string FundIdKey = "fund-id";

        public const string Message_InternalServerError = "خطایی در برنامه رخ داده است.";
        public const string Message_AccessDenied_Message = "با عرض پوزش،اجرای این درخواست مجاز نمی باشد.";

        public static TimeZoneInfo GetTehranTimeZoneInfo() => TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
    }
}
