using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
    public class Conversation : Entity
    {
        public Guid Id { get; set; }
        public ConversationType Type { get; set; }
        public string? Title { get; set; }
        public Guid CreateByUserId { get; set; }
        public virtual User CreateByUser { get; set; } = default!;
        public Guid? LastMessageId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
    public class ConversationParticipant : Entity
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = default!;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = default!;
        public DateTime JoinedAtUtc { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsMuted { get; set; } = false;
        public bool IsLeft { get; set; } = false;
        public bool IsArchived { get; set; } = false;
    }
    public class Message : Entity
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = default!;
        public Guid? SenderUserId { get; set; }
        public virtual User? SenderUser { get; set; } = default!;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAtUtc { get; set; }
        public MessageType Type { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsEdited { get; set; } = false;
        public DateTime? EditedAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
    public class MessageAttachment : Entity
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; } = default!;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Url { get; set; } = string.Empty;
    }
    public class GroupSettings : Entity
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string Description { get; set; } = string.Empty;
        public virtual Conversation Conversation { get; set; } = default!;
        public bool IsPublic { get; set; } = false;
        public string AvatarUrl { get; set; } = string.Empty;
    }

    public class AiConversation : Entity
    {
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = default!;
        public string AiModel { get; set; } = string.Empty;
        public double Temperature { get; set; } = 0.5;
        public string SystemPrompt { get; set; } = string.Empty;
    }
}