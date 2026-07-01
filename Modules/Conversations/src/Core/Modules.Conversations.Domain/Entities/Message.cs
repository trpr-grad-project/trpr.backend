using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities
{
    public class Message : Entity
    {
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public int SequenceNumber { get; private set; } = 1;
        public Guid ConversationId { get; private set; }
        public virtual Conversation Conversation { get; private set; } = default!;
        public Guid? SenderUserId { get; private set; }
        public virtual User? SenderUser { get; private set; } = default!;
        public string Content { get; private set; } = string.Empty;
        public DateTime SentAtUtc { get; private set; } = DateTime.UtcNow;
        public static Message Create(Conversation conversation, Guid userId, string content)
        {
            conversation.IncrementSequenceNumber();
            var message = new Message
            {
                Id = Guid.CreateVersion7(),
                ConversationId = conversation.Id,
                Conversation = conversation,
                SenderUserId = userId,
                Content = content,
                SequenceNumber = conversation.SequenceNumber
            };
            return message;
        }
    }
}