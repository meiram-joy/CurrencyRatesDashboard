using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.Entities;

namespace CurrencyRates.Domain.Currency.DomainEvent;

public class CurrencyRateAddedEvent : IDomainEvent
{
    public CurrencyRateAddedEvent(CurrencyRate newRate)
    {
        ID = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        NewRate = newRate;
    }

    public Guid ID { get; }
    public DateTime OccurredOn { get; }
    public CurrencyRate NewRate { get; }
}