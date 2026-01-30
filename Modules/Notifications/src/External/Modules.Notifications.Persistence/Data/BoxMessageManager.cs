using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.Correlation;
using Common.Domain;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Domain.Entities.Inbox;
using Modules.Notifications.Domain.Entities.Outbox;
using Newtonsoft.Json;

namespace Modules.Notifications.Persistence.Data
{
    public class BoxMessageManager(AppDbContext context, ICorrelationIdAccessor correlationIdAccessor) : IBoxMessageManager
    {
        public async Task InsertIntegrationEventToInBox(IntegrationEvent integrationEvent)
        {

            var correlationId = correlationIdAccessor.CorrelationId ?? Guid.Empty.ToString();
            var inBoxMessages = new InboxMessage()
            {
                CorrelationId = correlationId,
                Id = integrationEvent.Id,
                OccurredOnUtc = integrationEvent.CreatedOnUtc,
                Type = integrationEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(integrationEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            };

            context.InboxMessages.Add(inBoxMessages);
            await context.SaveChangesAsync();
        }
        public async Task InsertDomainEventsToOutbox(DomainEvent domainEvent)
        {
            var correlationId = correlationIdAccessor.CorrelationId ?? Guid.Empty.ToString();
            var outBoxMessages = new OutboxMessage()
            {
                CorrelationId = correlationId,
                Id = domainEvent.Id,
                OccurredOnUtc = domainEvent.CreatedOnUtc,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            };

            context.OutboxMessages.Add(outBoxMessages);
            await context.SaveChangesAsync();
        }

        public void InsertOutBoxMessages(ICollection<OutboxMessage> messages)
        {
            throw new NotImplementedException();
        }


    }
}