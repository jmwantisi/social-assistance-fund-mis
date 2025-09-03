using System.Net;
using System.Text.Json;
using socialAssistanceFundMIS.Common;

namespace socialAssistanceFundMIS.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
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
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                TraceId = context.TraceIdentifier,
                Message = _environment.IsDevelopment() ? exception.Message : "An error occurred while processing your request.",
                Details = _environment.IsDevelopment() ? exception.ToString() : null
            };

            switch (exception)
            {
                case ArgumentException:
                case ArgumentNullException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid request parameters.";
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Message = "Access denied. Please authenticate.";
                    break;

                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid operation.";
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = "The requested resource was not found.";
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An unexpected error occurred.";
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(result);
        }
    }

    public class ErrorResponse
    {
        public string TraceId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}

