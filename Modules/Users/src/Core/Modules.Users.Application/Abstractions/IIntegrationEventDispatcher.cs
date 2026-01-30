using Common.Domain;

namespace Modules.Users.Application.Abstractions
{
    public interface IIntegrationEventDispatcher
    {
        Task DispatchAsync(IntegrationEvent integrationEvent, CancellationToken cancellationTokenc = default);
    }

}