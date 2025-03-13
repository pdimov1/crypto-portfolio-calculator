namespace CryptoPortfolioCalculator
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
                _logger.LogError(ex, "MVC Exception: {Message} | Request: {Path}", ex.Message, context.Request.Path);

                context.Items["ErrorMessage"] = "An error occurred while processing your request.";
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}
