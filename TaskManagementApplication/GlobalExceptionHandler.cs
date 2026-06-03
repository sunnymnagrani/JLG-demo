using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace TaskManagementApplication
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            // Default to 500 Internal Server Error
            var statusCode = HttpStatusCode.InternalServerError;

            // Handle exceptions
            if (exception is KeyNotFoundException ||
               (exception is Exception && exception.Message == "Task Not Found"))
            {
                statusCode = HttpStatusCode.NotFound; // Returns 404
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;

            var responsePayload = new
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message,
                Detail = "An unexpected error occurred on the server."
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(responsePayload, options), cancellationToken);

            // Return true to indicate that this exception has been successfully handled
            return true;
        }
    }
}
