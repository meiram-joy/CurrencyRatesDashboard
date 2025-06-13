using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.ValueObjects;
using Entity = CurrencyRates.Domain.Common.Entity;

namespace CurrencyRates.Domain.Currency.Entities;

public class CurrencyRate : Entity
{
    public CurrencyCode CurrencyCode { get; private set; }
    public double Rate { get; private set; }
    public DateTime RateDate { get; private set; }
    
    private CurrencyRate(){}

    private CurrencyRate(CurrencyCode currencyCode, double rate, DateTime rateDate)
    {
        CurrencyCode = currencyCode;
        Rate = rate;
        RateDate = rateDate;
    }

    public static Result<CurrencyRate> Create(CurrencyCode currencyCode, double rate, DateTime rateDate)
    {
        if (currencyCode is null)
            return Result.Failure<CurrencyRate>("CurrencyCode is required.");
        if (rate <= 0)
            return Result.Failure<CurrencyRate>("Rate must be greater than zero.");
        if (rateDate == default)
            return Result.Failure<CurrencyRate>("Rate date is invalid.");
        
        return Result.Success(new CurrencyRate(currencyCode, rate, rateDate));
    }

    public void UpdateRate(double newRate, DateTime rateDate)
    {
        // if (newRate <= 0) throw new ArgumentOutOfRangeException(nameof(newRate));
        if (rateDate == default) throw new ArgumentException("Rate date cannot be default value.", nameof(rateDate));

        Rate = newRate;
        RateDate = rateDate;
    }
}