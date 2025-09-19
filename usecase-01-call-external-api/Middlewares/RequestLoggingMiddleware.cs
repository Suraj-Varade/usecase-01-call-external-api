using System.Diagnostics;
using System.Net;
using System.Text.Json;
using usecase_01_call_external_api.Errors;

namespace usecase_01_call_external_api.Middlewares;

public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        logger.LogInformation($"starting request: {context.Request.Path}");
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "error during request {Path}", context.Request.Path);
            await HandleExceptionAsync(context, e);
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation($"finished request: {context.Request.Path}, duration(ms): {stopwatch.ElapsedMilliseconds}");
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace);
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}