using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Modules.Notifications.Domain.Abstractions;
using Modules.Notifications.Domain.Entities.Outbox;

namespace Modules.Notifications.Persistence.Outbox;

public class PublishOutboxMessagesInterceptor(
    IHttpContextAccessor httpContextAccessor
) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        var correlationId = httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString()!;
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context, correlationId);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    public static void InsertOutboxMessages(DbContext context, string correlationId)
    {
        var outBoxMessages = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage()
            {
                CorrelationId = correlationId,
                Id = domainEvent.Id,
                OccurredOnUtc = domainEvent.CreatedOnUtc,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            }).ToList();

        context.Set<OutboxMessage>().AddRange(outBoxMessages);
    }
}
