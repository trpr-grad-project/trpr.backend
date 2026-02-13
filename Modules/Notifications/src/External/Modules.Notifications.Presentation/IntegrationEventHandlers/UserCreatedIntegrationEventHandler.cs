using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Services;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(UserService userService) : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task HandleAsync(UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await userService.CreateUser(integrationEvent.UserId, integrationEvent.UserName, integrationEvent.FirstName, integrationEvent.LastName, cancellationToken);
    }

}
