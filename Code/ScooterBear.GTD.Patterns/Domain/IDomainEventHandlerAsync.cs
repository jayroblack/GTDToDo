using System.Threading;
using System.Threading.Tasks;

namespace ScooterBear.GTD.Patterns.Domain
{
    public interface IDomainEventHandlerAsync<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
