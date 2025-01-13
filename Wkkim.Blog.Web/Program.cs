
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MySqlConnector;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

using Wkkim.Blog.Web.Data;
using Wkkim.Blog.Web.Middlewares;
using Wkkim.Blog.Web.Repositories;
using Serilog;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console() 
        .WriteTo.File("logs/dev_log.txt", rollingInterval: RollingInterval.Day)
        .MinimumLevel.Information()
        .CreateLogger();
}
else
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File("/var/log/aspblog/prod_log.txt", rollingInterval: RollingInterval.Day) 
        .MinimumLevel.Information()
        .CreateLogger();
}
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<LogIpAddressFilter>(); 
});

//builder.Services.AddLogging();

ILoggerFactory NullLoggerFactory = LoggerFactory.Create(builder => { });

// 로그 필터 설정
builder.Logging.ClearProviders(); // 모든 기본 로거 제거
builder.Logging.AddConsole();    // 콘솔 로깅 추가 (원하는 경우)
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None); // DB 명령어 로그 제거
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.None);           // 쿼리 관련 로그 제거

var connectionString = builder.Configuration.GetConnectionString("BlogDbConnectionString");
builder.Services.AddDbContext<BlogDbContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).UseLoggerFactory(NullLoggerFactory));

builder.Services.AddDbContext<AuthDBContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).UseLoggerFactory(NullLoggerFactory));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDBContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14); 
    options.SlidingExpiration = true; 
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    ForwardLimit = null 
});

app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();  

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
