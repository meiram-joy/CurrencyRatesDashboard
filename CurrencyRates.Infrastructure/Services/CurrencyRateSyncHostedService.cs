using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRates.Infrastructure.Services;

public class CurrencyRateSyncHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CurrencyRateSyncHostedService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

    public CurrencyRateSyncHostedService(
        IServiceProvider serviceProvider,
        ILogger<CurrencyRateSyncHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🟢 Служба синхронизации запущена");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<CurrencyRateSyncService>();

                await syncService.SyncRatesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Ошибка в службе синхронизации");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("🛑 Служба синхронизации остановлена");
    }
}