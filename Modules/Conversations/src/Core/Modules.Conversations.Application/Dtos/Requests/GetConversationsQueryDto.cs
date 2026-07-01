namespace Modules.Conversations.Application.Dtos.Requests;

public class GetConversationsQueryDto
{
    public int Limit { get; set; } = 20;
    public Guid? Cursor { get; set; }
}

public class CreateConversationRequestDto
{
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public List<Guid> ParticipantUserIds { get; set; } = [];
}