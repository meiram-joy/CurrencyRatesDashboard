using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Domain.Currency.Entities;

public class CurrencyRate : Entity
{
    public CurrencyCode CurrencyCode { get; private set; }
    public double Rate { get; private set; }
    public DateTime RateDate { get; private set; }
    
    public string Source { get; private set; }
    
    protected CurrencyRate(){}

    public CurrencyRate(CurrencyCode currencyCode, double rate, DateTime rateDate)
    {
        // if (rate <= 0 ) throw new ArgumentOutOfRangeException(nameof(rate));
        if (rateDate == default) throw new ArgumentException("Rate date cannot be default value.", nameof(rateDate));
        CurrencyCode = currencyCode;
        Rate = rate;
        RateDate = rateDate;
    }

    public void UpdateRate(double newRate, DateTime rateDate)
    {
        // if (newRate <= 0) throw new ArgumentOutOfRangeException(nameof(newRate));
        if (rateDate == default) throw new ArgumentException("Rate date cannot be default value.", nameof(rateDate));

        Rate = newRate;
        RateDate = rateDate;
    }
}