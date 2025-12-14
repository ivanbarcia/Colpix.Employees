using System.Net;
using System.Text.Json;
using EmployeeManagement.Domain.Exceptions;

namespace EmployeeManagement.Api.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorResponse();

        switch (exception)
        {
            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.StatusCode = context.Response.StatusCode;
                response.Message = unauthorizedException.Message;
                _logger.LogWarning(unauthorizedException, "Unauthorized access attempt");
                break;

            case NotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusCode = context.Response.StatusCode;
                response.Message = notFoundException.Message;
                _logger.LogWarning(notFoundException, "Resource not found");
                break;

            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.StatusCode = context.Response.StatusCode;
                response.Message = validationException.Message;
                response.Errors = validationException.Errors;
                _logger.LogWarning(validationException, "Validation error occurred");
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusCode = context.Response.StatusCode;
                response.Message = "An unexpected error occurred. Please try again later.";
                _logger.LogError(exception, "An unexpected error occurred");
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
