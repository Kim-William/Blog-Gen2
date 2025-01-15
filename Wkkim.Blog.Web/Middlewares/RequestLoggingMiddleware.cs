using System.IO;

using Newtonsoft.Json;

namespace Wkkim.Blog.Web.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var path = context.Request.Path.ToString();
            if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png") || path.EndsWith(".svg") ||
            path.EndsWith(".jpg") || path.EndsWith(".ico") || path.StartsWith("/_framework") || path.StartsWith("/_vs") || path.StartsWith("/css") ||
            path.StartsWith("/assets") || path.StartsWith("/image"))
            {
                await _next(context);
                return;
            }

            using (var scope = serviceProvider.CreateScope())
            {
                var logService = scope.ServiceProvider.GetRequiredService<LogService>();

                await logService.LogRequestAsync(context);
            }

            await _next(context);
        }
    }

}
