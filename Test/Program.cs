using CurrencyRates.Application.Interfaces;
using CurrencyRates.Application.Services;
using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Infrastructure;
using CurrencyRates.Infrastructure.Repositories;
using CurrencyRates.Infrastructure.Services.Export;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IExportService, ExportService>();

builder.Services.AddInfrastructure();

builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();

builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();

builder.Services.AddTransient<IExportService, ExportService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
