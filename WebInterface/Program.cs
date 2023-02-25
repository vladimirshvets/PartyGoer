using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using WebInterface.Data;
using WebInterface.Models.Settings;
using WebInterface.Repositories;
using WebInterface.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MSSQLConnection") ?? throw new InvalidOperationException("Connection string 'MSSQLConnection' not found.");

// Add data storages.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "chatinfo_";
});

// Add chat bot services.
List<BotConfiguration>? bots = builder.Configuration
    .GetSection("BotSettings")
    .Get<List<BotConfiguration>>();

builder.Services.AddScoped<AppCache>();
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddScoped<ChatBotProcessor>();

// Notice: Cannot create several hosted services of a ChatService
// with different implementation factories. The first one is created only.
// See https://github.com/dotnet/runtime/issues/38751 for details.
foreach (var bot in bots)
{
    // Therefore, create only one service for now.
    // ToDo:
    // Resolve it later.
    if (bot.AppId == "telegram")
    {
        builder.Services.AddHostedService<ChatService>(provider =>
        {
            var scope = provider.CreateScope();
            var botProcessor =
                scope.ServiceProvider.GetRequiredService<ChatBotProcessor>();
            return new ChatService(bot, botProcessor);
        });
    }
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Create logger instance.
// ToDo:
// Use DI.
Logger logger = LogManager.GetCurrentClassLogger();
logger.Debug($"Starting up the web application...");

app.Run();

