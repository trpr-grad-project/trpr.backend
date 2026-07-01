namespace Modules.Conversations.Application.Dtos.Responses;

public class ConversationDetailsResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public ConversationLastMessageDto? LastMessage { get; set; }
    public int LastReadSequence { get; set; }
    public int UnreadCount { get; set; }
}
