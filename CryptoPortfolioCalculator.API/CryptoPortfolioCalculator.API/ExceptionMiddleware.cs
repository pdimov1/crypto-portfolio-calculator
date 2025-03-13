using System.Net;
using System.Text.Json;

namespace CryptoPortfolioCalculator.API
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex is ArgumentException ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex is ArgumentException ? ex.Message : "An unexpected error occurred.",
                    RequestPath = context.Request.Path
                };

                _logger.LogError("Error Response: {@Response}", response);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}