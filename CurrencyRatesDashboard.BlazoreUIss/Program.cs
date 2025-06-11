using CurrencyRates.Application.Interfaces;
using CurrencyRates.Application.Services;
using CurrencyRates.Infrastructure;
using CurrencyRates.Infrastructure.External;
using CurrencyRates.Infrastructure.Services.Export;
using CurrencyRatesDashboard.BlazoreUIss.Components;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddScoped<ExportService>();
builder.Services.AddInfrastructure();
// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<FileUtil>();
builder.Services.AddScoped<ICurrencyRateApiClient, CurrencyRateApiClient>();
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddScoped<ThemeService>(); 

builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
