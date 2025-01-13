using Microsoft.Extensions.Logging;

public class LoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public void LogInformation(HttpContext httpContext)
    {
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        var url = httpContext.Request.Path;

        _logger.LogInformation($"IP:[{ipAddress}] URL:[{url}]");
    }
}
