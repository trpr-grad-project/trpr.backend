using Common.Domain;

namespace Modules.Notifications.Application.Abstractions;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}