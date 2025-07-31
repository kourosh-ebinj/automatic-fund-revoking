using System;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebCore.Extensions;

namespace WebCore.Helpers;

public sealed class GlobalErrorHandler : IExceptionHandler
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<GlobalErrorHandler> _logger;

    public GlobalErrorHandler(IHostEnvironment environment, ILogger<GlobalErrorHandler> logger)
    {
        _environment = environment;
        _logger = logger;

    }

    public async Task WriteDevelopmentResponse(HttpContext httpContext)
        => await WriteResponse(httpContext, includeDetails: true);

    public async Task WriteProductionResponse(HttpContext httpContext)
        => await WriteResponse(httpContext, includeDetails: false);

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        try
        {
            if (_environment.IsProduction())
                await WriteProductionResponse(httpContext);
            else
                await WriteDevelopmentResponse(httpContext);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Critical, ex, "Global Error handler encountered with an error");

            return false;
        }
    }

    private async Task WriteResponse(HttpContext httpContext, bool includeDetails)
    {
        // Try and retrieve the error from the ExceptionHandler middleware
        var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
        var ex = exceptionDetails?.Error;

        // Should always exist, but best to be safe!
        if (ex == null) await Task.CompletedTask;

        // ProblemDetails has it's own content type
        httpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;

        string? title = ex.Message;
        switch (ex)
        {
            case CustomBadRequestException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            case CustomUnauthorizedException:
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                break;
            case CustomForbiddenException:
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                break;
            case CustomNotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            case CustomTooManyRequestsException:
                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                break;
            default:
                title = "متاسفیم، مشکلی در سرور رخ داده است. لطفاً بعداً دوباره تلاش کنید.";
                break;
        }
        // Get the details to display, depending on whether we want to expose the raw exception
        string? details = includeDetails ? ex.ToString() : default;

        // This is often very handy information for tracing the specific request
        var correlationId = httpContext.GetCorrelationId();

        var problem = APIResultHelper.RestResultBody($"{title}", correlationId, details, ex.Data.Values.Cast<string>());

        await httpContext.Response.WriteAsJsonAsync(problem);

        ex.Data.Add(nameof(title), ex.Message);
        ex.Data.Add(nameof(details), ex.ToString());
        _logger.LogError(ex.Data);
    }
}
