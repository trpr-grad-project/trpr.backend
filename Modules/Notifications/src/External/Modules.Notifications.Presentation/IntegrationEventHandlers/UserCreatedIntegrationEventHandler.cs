using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Users.Contracts.IntegrationEvents;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger) : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public Task HandleAsync(UserCreatedIntegrationEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("User with identifier recived in the notification module {identifier}", domainEvent.UserName);
        return Task.CompletedTask;
    }

}
