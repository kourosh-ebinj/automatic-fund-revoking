using System.ComponentModel;

namespace Core.Enums;

public enum ExceptionCodeEnum : short
{
    [Description("درخواست نامعتبر")]
    BadRequest = 400,
    [Description("عدم احراز هویت")]
    Unauthorized = 401,
    [Description("دسترسی غیر مجاز")]
    Forbidden = 403,
    [Description("مورد یافت نشد")]
    NotFound = 404,
    [Description("تعداد درخواست های بیش از حد مجاز")]
    TooManyRequests = 429,
    [Description("خطای داخلی سرور")]
    InternalServerError = 500
}
