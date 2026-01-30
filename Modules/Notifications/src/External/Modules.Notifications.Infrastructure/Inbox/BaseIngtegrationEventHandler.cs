using Common.Domain;
using Modules.Notifications.Infrastructure.Data;
using Newtonsoft.Json;
using Rebus.Handlers;

namespace Modules.Notifications.Infrastructure.Inbox;

public class BaseIngtegrationEventHandler<T>(AppDbContext context) : IHandleMessages<T> where T : IntegrationEvent
{
    public async Task Handle(T message)
    {
        var inBoxMessages = new InboxMessage()
        {
            CorrelationId = message.CorrelationId,
            Id = message.Id,
            OccurredOnUtc = message.CreatedOnUtc,
            Type = message.GetType().Name,
            Content = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            })
        };

        context.InboxMessages.Add(inBoxMessages);
        await context.SaveChangesAsync();
    }

}
