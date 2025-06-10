using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.Entities;

namespace CurrencyRates.Domain.Currency.DomainEvent;

public class CurrencyRateAddedEvent : IDomainEvent
{
    public CurrencyRateAddedEvent(CurrencyRate newRate)
    {
        throw new NotImplementedException();
    }

    public Guid ID { get; }
    public DateTime OccurredOn { get; }
}