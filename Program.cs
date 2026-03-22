using AccountingPortfolio.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file + console logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()                 // log info, warnings, errors
    .WriteTo.Console()                          // optional: also log to console
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day) // one file per day
    .CreateLogger();

// Use Serilog for the host
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllersWithViews();


// Register queue as singleton and as hosted service
builder.Services.AddSingleton<EmailQueueService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<EmailQueueService>());
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();