using Common.Domain;

namespace Modules.Notifications.Application.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    }

}