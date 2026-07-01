using Modules.Conversations.Domain.Abstractions;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Domain.Entities
{
    public class Conversation : Entity
    {
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public int SequenceNumber { get; private set; } = 0;
        public string? ImageUrl { get; private set; }
        public string? Title { get; private set; }
        public Guid CreateByUserId { get; private set; }
        public virtual User CreateByUser { get; private set; } = default!;
        public virtual ICollection<ConversationParticipant> Participants { get; private set; } = [];
        public virtual ICollection<Message> Messages { get; private set; } = [];
        public void IncrementSequenceNumber()
        {
            SequenceNumber++;
        }
        public static Conversation Create(string? title, string? imageUrl, Guid createByUserId)
        {
            var conversation = new Conversation
            {
                Id = Guid.CreateVersion7(),
                Title = title,
                ImageUrl = imageUrl,
                CreateByUserId = createByUserId
            };
            return conversation;
        }
    }
}