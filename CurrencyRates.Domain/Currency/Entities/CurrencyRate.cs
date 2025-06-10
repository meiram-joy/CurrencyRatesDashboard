using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Domain.Currency.Entities;

public class CurrencyRate : Entity
{
    public CurrencyCode CurrencyCode { get; private set; }
    public decimal Rate { get; private set; }
    public DateTime RateDate { get; private set; }
    public string Source { get; private set; }
    public bool IsOfficial { get; private set; }
    
    protected CurrencyRate(){}

    public CurrencyRate(CurrencyCode currencyCode, decimal rate, DateTime rateDate, string source, bool isOfficial)
    {
        if (rate <= 0 ) throw new ArgumentOutOfRangeException(nameof(rate));
        CurrencyCode = currencyCode;
        Rate = rate;
        RateDate = rateDate;
        Source = source;
        IsOfficial = isOfficial;
    }

    public void UpdateRate(decimal newRate, DateTime rateDate)
    {
        if (newRate <= 0) throw new ArgumentOutOfRangeException(nameof(newRate));
        if (rateDate == default) throw new ArgumentException("Rate date cannot be default value.", nameof(rateDate));

        Rate = newRate;
        RateDate = rateDate;
    }
}