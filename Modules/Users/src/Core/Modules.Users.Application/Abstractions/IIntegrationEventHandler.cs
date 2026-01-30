using Common.Domain;

namespace Modules.Users.Application.Abstractions;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}