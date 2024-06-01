global using FastEndpoints;
using InverterMon.Server.BatteryService;
using InverterMon.Server.InverterService;
using InverterMon.Server.Persistance;
using InverterMon.Server.Persistance.Settings;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Avoid parsing issues with non-English cultures
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
_ = int.TryParse(builder.Configuration["LaunchSettings:WebPort"] ?? "80", out var port);
builder.WebHost.ConfigureKestrel(o => o.Listen(IPAddress.Any, port));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<UserSettings>();
builder.Services.AddSingleton<CommandQueue>();
builder.Services.AddSingleton<JkBms>();
builder.Services.AddSingleton<Database>();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<CommandExecutor>();
    builder.Services.AddHostedService<StatusRetriever>();
}

builder.Services.AddFastEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// Enable CORS
app.UseCors("AllowAll");

app.UseRouting();
app.UseAuthorization();

app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

app.MapFallbackToFile("index.html");

app.Run();