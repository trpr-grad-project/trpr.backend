namespace Modules.Conversations.Application.Dtos.Requests;

public class LastConversationMessageDto
{
    public Guid ConversationId { get; set; }
    public Guid LastMessageId { get; set; }
}
