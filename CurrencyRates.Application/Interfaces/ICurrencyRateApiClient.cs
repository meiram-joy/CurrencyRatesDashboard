using CurrencyRates.Domain.Currency.Aggregates;

namespace CurrencyRates.Application.Interfaces;

public interface ICurrencyRateApiClient
{
    Task<IReadOnlyCollection<CurrencyRateAggregate>> GetLatestRatesAsync();
}