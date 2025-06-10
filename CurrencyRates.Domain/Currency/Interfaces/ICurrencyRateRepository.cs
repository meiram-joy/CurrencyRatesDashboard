using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.Entities;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Domain.Currency.Interfaces;

public interface ICurrencyRateRepository
{
    Task<IReadOnlyList<CurrencyRateAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CurrencyRateAggregate?> GetByCodeAsync(CurrencyCode code, CancellationToken cancellationToken = default);
    Task AddAsync(CurrencyRateAggregate rate, CancellationToken cancellationToken = default);
    Task UpdateAsync(CurrencyRateAggregate rate, CancellationToken cancellationToken = default);

}