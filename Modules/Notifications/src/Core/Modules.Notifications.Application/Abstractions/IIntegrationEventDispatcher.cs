using Common.Domain;

namespace Modules.Notifications.Application.Abstractions
{
    public interface IIntegrationEventDispatcher
    {
        Task DispatchAsync(IntegrationEvent integrationEvent, CancellationToken cancellationTokenc = default);
    }

}