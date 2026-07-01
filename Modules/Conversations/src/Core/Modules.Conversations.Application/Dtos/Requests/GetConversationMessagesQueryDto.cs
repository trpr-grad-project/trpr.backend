namespace Modules.Conversations.Application.Dtos.Requests;

public class GetConversationMessagesQueryDto
{
    public int Limit { get; set; } = 50;
    public int? AfterSequence { get; set; }
}
