using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WCCG.DentalMock.UI.Constants;
using WCCG.DentalMock.UI.Extensions;
using Task = System.Threading.Tasks.Task;

namespace WCCG.DentalMock.UI.Middleware;

public class ResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseMiddleware> _logger;

    public ResponseMiddleware(RequestDelegate next, ILogger<ResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        AddResponseHeaders(context);
        AddOperationIdHeader(context);

        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.UnexpectedError(exception);

        var problemDetails = new ProblemDetails
        {
            Title = "Unexpected error",
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(problemDetails)));
    }

    private static void AddResponseHeaders(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(RequestHeaderKeys.RequestId, out var requestId))
        {
            context.Response.Headers.TryAdd(RequestHeaderKeys.RequestId, requestId);
        }

        if (context.Request.Headers.TryGetValue(RequestHeaderKeys.CorrelationId, out var correlationId))
        {
            context.Response.Headers.TryAdd(RequestHeaderKeys.CorrelationId, correlationId);
        }
    }

    private static void AddOperationIdHeader(HttpContext context)
    {
        context.Response.Headers.TryAdd("X-Operation-Id", Activity.Current?.TraceId.ToString());
    }
}
