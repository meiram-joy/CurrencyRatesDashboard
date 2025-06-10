using CurrencyRates.Application.Queries;
using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Infrastructure.External;
using CurrencyRates.Infrastructure.Mappings;
using CurrencyRates.Infrastructure.Repositories;
using CurrencyRates.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRates.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
        services.AddHttpClient<CurrencyRateApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7020/"); // или адрес твоего mock-сервера
        });
        
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetCurrencyRatesQuery).Assembly)
        );
        services.AddScoped<CurrencyRateSyncService>();
        services.AddHostedService<CurrencyRateSyncHostedService>();
        
        return services;
    }
}