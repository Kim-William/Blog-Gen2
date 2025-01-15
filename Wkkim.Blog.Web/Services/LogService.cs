using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wkkim.Blog.Web.Data;
using Wkkim.Blog.Web.Models;
using Wkkim.Blog.Web.Models.Domain;

public class LogService
{
    private readonly BlogDbContext _dbContext;

    public LogService(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogRequestAsync(HttpContext context)
    {
        var id = context.Connection.Id;

        var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                  ?? context.Connection.RemoteIpAddress?.ToString();

        var method = context.Request.Method;

        var fullUrl = $"{context.Request.Path}{context.Request.QueryString}";


        var log = new UserLog
        {
            Id = Guid.NewGuid().ToString(),
            UserId = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Anonymous",
            IPAddress = clientIp,
            RequestUrl = fullUrl,
            HttpMethod = method,
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            ResponseCode = context.Response.StatusCode,
            AccessTime = DateTime.Now
        };

        _dbContext.UserLogs.Add(log);
        await _dbContext.SaveChangesAsync();
    }
}