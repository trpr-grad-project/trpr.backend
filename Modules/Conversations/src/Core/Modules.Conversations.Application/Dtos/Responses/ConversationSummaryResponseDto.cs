namespace Modules.Conversations.Application.Dtos.Responses;

public class ConversationSummaryResponseDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public ConversationLastMessageDto? LastMessage { get; set; }
    public int LastReadSequence { get; set; }
    public int UnreadCount { get; set; }
}
