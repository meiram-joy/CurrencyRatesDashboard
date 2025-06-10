using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.DomainEvent;
using CurrencyRates.Domain.Currency.Entities;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Domain.Currency.Aggregates;

public class CurrencyRateAggregate : AggregateRoot
{
    private readonly List<CurrencyRate> _currencyRates = new ();
    
    public IReadOnlyCollection<CurrencyRate> CurrencyRates => _currencyRates.AsReadOnly();

    public void AddOrUpdateQuote(CurrencyCode currencyCode, decimal rate, DateTime rateDate, string source, bool isOfficial)
    {
        if (currencyCode == null) throw new ArgumentNullException(nameof(currencyCode));
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate));
        if (rateDate == default) throw new ArgumentException("Rate date cannot be default value.", nameof(rateDate));
        if (string.IsNullOrWhiteSpace(source)) throw new ArgumentException("Source cannot be null or empty.", nameof(source));

        var existingRate = _currencyRates.FirstOrDefault(r => r.CurrencyCode.Equals(currencyCode) && r.RateDate.Date == rateDate.Date);
        
        if (existingRate != null)
        {
            existingRate.UpdateRate(rate, rateDate);
        }
        else
        {
            var newRate = new CurrencyRate(currencyCode, rate, rateDate, source, isOfficial);
            _currencyRates.Add(newRate);
            AddDomainEvent(new CurrencyRateAddedEvent(newRate));
        }
    }
}