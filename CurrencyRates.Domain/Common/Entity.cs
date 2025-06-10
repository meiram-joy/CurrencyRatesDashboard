namespace CurrencyRates.Domain.Common;

public abstract class Entity
{
    public Guid ID { get; protected set; } = Guid.NewGuid();
}