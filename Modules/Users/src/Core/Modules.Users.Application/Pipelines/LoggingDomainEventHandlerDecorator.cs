using Microsoft.Extensions.Logging;
using System.Text.Json;
using Modules.Users.Domain.Abstractions;
using Modules.Users.Application.Abstractions;

namespace Modules.Users.Application.Pipelines;

public class LoggingDomainEventHandlerDecorator<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> innerHandler,
    ILogger<LoggingDomainEventHandlerDecorator<TDomainEvent>> logger) : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task HandleAsync(TDomainEvent notification, CancellationToken cancellationToken)
    {

        string handlerName = innerHandler.GetType().Name;
        string eventData = JsonSerializer.Serialize(notification);
        logger.LogInformation("Handling domain event {EventType} with handler {HandlerName}. Event Data: {EventData}",
            typeof(TDomainEvent).Name, handlerName, JsonSerializer.Serialize(eventData));
        await innerHandler.HandleAsync(notification, cancellationToken);
        logger.LogInformation("Handled domain event {EventType} with handler {HandlerName}.",
            typeof(TDomainEvent).Name, handlerName);
    }

}