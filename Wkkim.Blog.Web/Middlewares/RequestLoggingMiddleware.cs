using System.IO;

namespace Wkkim.Blog.Web.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png") ||
            path.EndsWith(".jpg") || path.EndsWith(".ico") || path.StartsWith("/_framework") || path.StartsWith("/_vs"))
            {
                await _next(context);
                return;
            }

            var id = context.Connection.Id;

            var request = context.Request;
            var method = context.Request.Method; 

            var fullUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            _logger.LogInformation($"ID:{id}|Method:{method}|URL:{fullUrl}|IP:{ipAddress}");


            await _next(context);
        }
    }

}
