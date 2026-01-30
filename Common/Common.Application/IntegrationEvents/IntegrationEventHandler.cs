using Common.Domain;
using Modules.Notifications.Application.Abstractions;

namespace Common.Application.IntegrationEvents
{
    public abstract class IntegrationEventHandler<TEvent> : IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
    {
        public abstract Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
}