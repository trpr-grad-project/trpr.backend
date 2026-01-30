using Common.Domain;
using Modules.Notifications.Application.Abstractions;
using Rebus.Handlers;

namespace Modules.Notifications.Presentation;

public class BaseIngtegrationEventHandler<T>(IBoxMessageManager boxMessageManager) : IHandleMessages<T> where T : IntegrationEvent
{
    public async Task Handle(T message)
    {
        await boxMessageManager.InsertIntegrationEventToInBox(message);
    }

}
