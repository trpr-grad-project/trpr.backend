using Common.Domain;

namespace Common.Application.IntegrationEvents
{
    public interface IIntegrationEventDispatcher
    {
        Task DispatchAsync(IntegrationEvent integrationEvent, CancellationToken cancellationTokenc = default);
    }

}