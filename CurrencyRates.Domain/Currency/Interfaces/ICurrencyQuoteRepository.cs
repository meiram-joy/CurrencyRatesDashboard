using CurrencyRates.Domain.Currency.Entities;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Domain.Currency.Interfaces;

public interface ICurrencyQuoteRepository
{
    Task<IReadOnlyList<CurrencyRate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CurrencyRate?> GetByCodeAsync(CurrencyCode code, CancellationToken cancellationToken = default);
    Task AddAsync(CurrencyRate quote, CancellationToken cancellationToken = default);
    Task UpdateAsync(CurrencyRate quote, CancellationToken cancellationToken = default);
}