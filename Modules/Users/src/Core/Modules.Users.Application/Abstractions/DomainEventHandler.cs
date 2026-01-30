
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Application.Abstractions;

public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    public abstract Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}