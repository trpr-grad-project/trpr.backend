using Modules.Notifications.Domain.Abstractions;

namespace Modules.Notifications.Application.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    }

}