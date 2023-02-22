using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using WebInterface.Data;
using WebInterface.Models.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MSSQLConnection") ?? throw new InvalidOperationException("Connection string 'MSSQLConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
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
Logger logger = LogManager.GetCurrentClassLogger();
logger.Debug($"Starting up the web application...");

// Create and start chat bot instance.
var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString);
ApplicationDbContext dbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);

string tgAccessToken = builder.Configuration.GetValue<string>("BotSettings:Telegram:General:AccessToken") ?? throw new InvalidOperationException("Telegram API access token not found.");
await new ChatBotProcessor(dbContext).InitializeBot("telegram", tgAccessToken);

app.Run();
