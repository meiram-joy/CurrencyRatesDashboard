using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRates.Domain.Common;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id)
    {
        ID = id;
    }

    public AggregateRoot()
    {
        
    }

    private List<IDomainEvent> _domainEvents = new ();

    [NotMapped] 
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (_domainEvents == null)
            _domainEvents = new List<IDomainEvent>();

        if (_domainEvents.Any(s => s.GetType().Name == domainEvent.GetType().Name && s.ID == domainEvent.ID))
            return;

        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents() 
    {
        if (_domainEvents != null && _domainEvents.Count > 0)
        {
            _domainEvents.Clear();
        }
    }
}