using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities;

public class AiMessage : Entity
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public AiMessageRole Role { get; set; }
    public virtual AiConversation AiConversation { get; set; } = default!;
    public string Contnet { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public static AiMessage Create(AiConversation conversation, string content, AiMessageRole role)
    {
        var parentMessage = new AiMessage()
        {
            Id = Guid.CreateVersion7(),
            ConversationId = conversation.Id,
            AiConversation = conversation,
            Role = role,
            Contnet = content,
            CreatedOnUtc = DateTime.UtcNow
        };
        return parentMessage;
    }
}

