using System.IO;

using Newtonsoft.Json;

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
            path.EndsWith(".jpg") || path.EndsWith(".ico") || path.StartsWith("/_framework") || path.StartsWith("/_vs") || path.StartsWith("/css") || 
            path.StartsWith("/assts"))
            {
                await _next(context);
                return;
            }

            var id = context.Connection.Id;

            var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                      ?? context.Connection.RemoteIpAddress?.ToString();

            var request = context.Request;
            var method = context.Request.Method; 

            var fullUrl = $"{context.Request.Path}{context.Request.QueryString}";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            var logData = new
            {
                id = id,            
                method = method,    
                url = fullUrl,      
                ip = ipAddress      
            };

            // Serialize the object to a JSON string
            string jsonString = JsonConvert.SerializeObject(logData);

            // Save or log the JSON string
            Console.WriteLine(jsonString);

            _logger.LogInformation(jsonString);


            await _next(context);
        }
    }

}
