namespace Modules.Notifications.Domain.Entities.Inbox;

public class InboxConsumerMessage
{
    public Guid Id { get; set; }
    public string HandlerName { get; set; } = string.Empty;
}
