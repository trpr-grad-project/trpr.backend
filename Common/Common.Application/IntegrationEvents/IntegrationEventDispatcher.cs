using Common.Domain;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Abstractions;

namespace Common.Application.IntegrationEvents;

public class IntegrationEventDispatcher(IServiceProvider serviceProvider) : IIntegrationEventDispatcher
{
    public async Task DispatchAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(integrationEvent.GetType());

        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("HandleAsync");

            if (method is not null)
            {
                await (Task)method.Invoke(handler, new object[] { integrationEvent, cancellationToken })!;
            }
        }
    }
}
