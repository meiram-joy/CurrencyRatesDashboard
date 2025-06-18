using CurrencyRates.Application.Commands;
using CurrencyRates.Application.Queries;
using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Infrastructure.External;
using CurrencyRates.Infrastructure.Mappings;
using CurrencyRates.Infrastructure.Repositories;
using CurrencyRates.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.Services;
using CurrencyRates.Infrastructure.Repositories.Auth;
using CurrencyRates.Infrastructure.Services.Auth;
using Microsoft.Extensions.Configuration;

namespace CurrencyRates.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration config)
    {
        services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
        services.AddSingleton<IJwtTokenService, JwtTokenGenerator>();
        
        services.AddHttpClient<CurrencyRateApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://8b29c522-a109-4b79-996f-a2120dbc9d5b.mock.pstmn.io/"); 
        });

        var handlerAssembly = Assembly.Load("CurrencyRates.Application");
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(handlerAssembly));

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
      
        services.AddScoped<CurrencyRateSyncService>();
        services.AddHostedService<CurrencyRateSyncHostedService>();
        
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthDomainService, AuthDomainService>();
        return services;
    }
}