namespace Modules.Conversations.Application.Dtos.Responses;

public class MessageListItemDto
{
    public Guid Id { get; set; }
    public int SequenceNumber { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? SenderUserId { get; set; }
    public DateTime SentAtUtc { get; set; }
    public Guid ConversationId { get; set; }
}
