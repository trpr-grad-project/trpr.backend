namespace Modules.Conversations.Application.Dtos.Requests;

public class CreateConversationRequestDto
{
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public List<Guid> ParticipantUserIds { get; set; } = [];
}