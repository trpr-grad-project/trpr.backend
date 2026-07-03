namespace Modules.Conversations.Application.Dtos.Requests;

public class GetConversationsQueryDto
{
    public int Limit { get; set; } = 20;
    public Guid? Cursor { get; set; }
}
