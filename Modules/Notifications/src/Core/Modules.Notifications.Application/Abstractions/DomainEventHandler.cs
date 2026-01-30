using Common.Domain;

namespace Modules.Notifications.Application.Abstractions;

public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public abstract Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}