using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using PartyGoer.ChatBot;
using WebInterface;
using WebInterface.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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
LogManager.LoadConfiguration("..//..//..//nlog_web.config");
Logger logger = LogManager.GetCurrentClassLogger();
logger.Debug($"Starting up the web application");

// Create and start chat bot instance.
string tgAccessToken = builder.Configuration.GetValue<string>(
    "BotSettings:Telegram:General:AccessToken");
ChatBotFactory botFactory = new ChatBotFactory();
IChatBot tgBot = botFactory.GetChatBot("telegram", tgAccessToken);

using CancellationTokenSource cts = new();
tgBot.TestConnectionAsync(cts);
tgBot.StartBot(cts);

app.Run();
