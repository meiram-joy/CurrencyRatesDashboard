using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Infrastructure.External;
using Microsoft.Extensions.Logging;

namespace CurrencyRates.Infrastructure.Services;

public class CurrencyRateSyncService
{
    private readonly CurrencyRateApiClient _apiClient;
    private readonly ICurrencyRateRepository _repository;
    private readonly ILogger<CurrencyRateSyncService> _logger;

    public CurrencyRateSyncService(
        CurrencyRateApiClient apiClient,
        ICurrencyRateRepository repository,
        ILogger<CurrencyRateSyncService> logger)
    {
        _apiClient = apiClient;
        _repository = repository;
        _logger = logger;
    }

    public async Task SyncRatesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🔄 Начинается синхронизация курсов валют...");

        var rates = await _apiClient.GetLatestRatesAsync();

        foreach (var rate in rates)
        {
            var existing = await _repository.GetByCodeAsync(rate.CurrencyCode, cancellationToken);

            if (existing is null)
            {
                _logger.LogInformation("Добавление курса: {Code}", rate.CurrencyCode);
                await _repository.AddAsync(rate, cancellationToken);
            }
            else
            {
                _logger.LogInformation("Обновление курса: {Code}", rate.CurrencyCode);
                await _repository.UpdateAsync(rate, cancellationToken);
            }
        }

        _logger.LogInformation("✅ Синхронизация завершена.");
    }
}