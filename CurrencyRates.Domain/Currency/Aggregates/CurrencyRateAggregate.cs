using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.DomainEvent;
using CurrencyRates.Domain.Currency.Entities;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Domain.Currency.Aggregates;

public class CurrencyRateAggregate : AggregateRoot
{
    private readonly List<CurrencyRate> _currencyRates = new ();
    
    public IReadOnlyCollection<CurrencyRate> CurrencyRates => _currencyRates.AsReadOnly();

    public void  AddOrUpdateQuote(CurrencyCode currencyCode, double rate, DateTime rateDate)
    {
        if (currencyCode == null) throw new ArgumentNullException(nameof(currencyCode));
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate));
        if (rateDate == default) throw new ArgumentException("Rate date cannot be default value.", nameof(rateDate));

        var existingRate = _currencyRates.FirstOrDefault(r => r.CurrencyCode.Equals(currencyCode) && r.RateDate.Date == rateDate.Date);
        
        if (existingRate != null)
        {
            existingRate.UpdateRate(rate, rateDate);
        }
        else
        {
            var newRate = CurrencyRate.Create(currencyCode, rate, rateDate);
            if (newRate.IsFailure)
                throw new InvalidOperationException(newRate.Error);
            
            _currencyRates.Add(newRate.Value);
            AddDomainEvent(new CurrencyRateAddedEvent(newRate.Value));
        }
    }
    public static Result<CurrencyRateAggregate> Create(CurrencyCode code, string name, double rate, DateTime updatedAt)
    {
        if (string.IsNullOrWhiteSpace(code.Code))
            return Result.Failure<CurrencyRateAggregate>("Code is required.");

        if (rate <= 0)
            return Result.Failure<CurrencyRateAggregate>("Rate must be greater than zero.");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<CurrencyRateAggregate>("Name is required.");

        if (updatedAt == default)
            return Result.Failure<CurrencyRateAggregate>("RetrievedAt must be set.");
        
        var aggregate = new CurrencyRateAggregate
        {
            CurrencyCode = code,
            Name = name,
            Rate = rate,
            RetrievedAt = updatedAt
        };

        return Result.Success(aggregate);
    }

    public CurrencyCode CurrencyCode { get; private set; }
    public string Name { get; private set; }
    public double Rate { get; private set; }
    public DateTime RetrievedAt { get; private set; }
}