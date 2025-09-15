using System.Net;
using System.Text.Json;
using Malte.Clean.API.DTOs;
using Malte.Clean.Domain.Exceptions;

namespace Malte.Clean.API.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ApiErrorResponse
        {
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            case ImageLimitExceededException imageLimitEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Title = "Image Limit Exceeded";
                errorResponse.Status = response.StatusCode;
                errorResponse.Detail = imageLimitEx.Message;
                break;

            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Title = "Validation Error";
                errorResponse.Status = response.StatusCode;
                errorResponse.Detail = "One or more validation errors occurred";
                errorResponse.Errors = validationEx.Errors;
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Title = "Resource Not Found";
                errorResponse.Status = response.StatusCode;
                errorResponse.Detail = notFoundEx.Message;
                break;

            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Title = "Invalid Argument";
                errorResponse.Status = response.StatusCode;
                errorResponse.Detail = argEx.Message;
                break;

            case InvalidOperationException opEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Title = "Invalid Operation";
                errorResponse.Status = response.StatusCode;
                errorResponse.Detail = opEx.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Title = "Internal Server Error";
                errorResponse.Status = response.StatusCode;
                errorResponse.Detail = "An error occurred while processing your request";
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await response.WriteAsync(jsonResponse);
    }
}