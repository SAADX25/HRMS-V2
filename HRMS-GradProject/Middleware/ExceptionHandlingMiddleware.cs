using System.Net;
using System.Text.Json;
using Application.Common;

namespace HRMS_API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ArgumentNullException      => (HttpStatusCode.BadRequest,      "A required value was missing."),
            ArgumentException          => (HttpStatusCode.BadRequest,      exception.Message),
            KeyNotFoundException       => (HttpStatusCode.NotFound,        exception.Message),
            InvalidOperationException  => (HttpStatusCode.Conflict,        exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized,   "You are not authorized to perform this action."),
            _                          => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse.Fail(message, new List<string> { exception.Message });

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
