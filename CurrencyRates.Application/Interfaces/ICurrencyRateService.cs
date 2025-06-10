using CurrencyRates.Application.DTOs;

namespace CurrencyRates.Application.Interfaces;

public interface ICurrencyRateService
{
    Task<IReadOnlyList<CurrencyRateDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task RefreshRatesAsync(CancellationToken cancellationToken = default);
}