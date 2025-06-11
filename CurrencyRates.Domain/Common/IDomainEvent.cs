using CurrencyRates.Domain.Currency.Entities;
using MediatR;

namespace CurrencyRates.Domain.Common;

public interface IDomainEvent : INotification
{
    Guid ID { get; }
    DateTime OccurredOn { get; }
    public CurrencyRate NewRate { get; }
}