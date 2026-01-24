namespace Modules.Notifications.Domain.Entities.Outbox;

public class OutboxConsumerMessage
{
    public Guid Id { get; set; }
    public string HandlerName { get; set; } = string.Empty;
}
