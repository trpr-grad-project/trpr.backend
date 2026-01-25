using Common.Application.EventBus;

namespace Common.Infrastructure.EventBus;

internal class EventBus : IEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        throw new NotImplementedException();
    }

}
