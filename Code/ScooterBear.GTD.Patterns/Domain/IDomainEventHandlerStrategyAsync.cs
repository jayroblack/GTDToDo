using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ScooterBear.GTD.Patterns.Domain
{
    public interface IDomainEventHandlerStrategyAsync<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        Task HandleEventsAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }

    public class DomainEventHandlerStrategyAsync<TDomainEvent> : IDomainEventHandlerStrategyAsync<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private readonly IList<IDomainEventHandlerAsync<TDomainEvent>> _eventHandlers;
        private readonly ILogger<DomainEventHandlerStrategyAsync<TDomainEvent>> _logger;

        public DomainEventHandlerStrategyAsync(IList<IDomainEventHandlerAsync<TDomainEvent>> eventHandlers,
            ILogger<DomainEventHandlerStrategyAsync<TDomainEvent>> logger)
        {
            _eventHandlers = eventHandlers ?? throw new ArgumentNullException(nameof(eventHandlers));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleEventsAsync(TDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            foreach (var myEvent in _eventHandlers)
            {
                try
                {
                    await myEvent.HandleAsync(domainEvent, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, new EventId(1234, "Error Handling Events"), domainEvent, ex,
                        (handler, exception) => $"Error executing {myEvent.GetType().ToString()} {ex.Message}");
                }
            }
        }
    }
}
