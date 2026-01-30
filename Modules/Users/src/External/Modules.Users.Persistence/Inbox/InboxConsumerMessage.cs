namespace Modules.Users.Persistence.Inbox;

public class InboxConsumerMessage
{
    public Guid Id { get; set; }
    public string HandlerName { get; set; } = string.Empty;
}
