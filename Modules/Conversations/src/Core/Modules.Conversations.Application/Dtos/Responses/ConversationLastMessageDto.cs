namespace Modules.Conversations.Application.Dtos.Responses;

public class ConversationLastMessageDto
{
    public Guid Id { get; set; }
    public int SequenceNumber { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid? SenderId { get; set; }
    public DateTime SentAt { get; set; }
}
