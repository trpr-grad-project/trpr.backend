using System.Text.Json;
using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;

namespace Modules.Notifications.Presentation.IntegrationEventHandlers;

public class SendMessageIntegrationEventHandler(ILogger<SendMessageIntegrationEventHandler> logger) : IIntegrationEventHandler<SendMessageIntegrationEvent>
{
    public Task HandleAsync(SendMessageIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("integration event json {integrationEvent}", integrationEvent);
        return Task.CompletedTask;
    }
}
