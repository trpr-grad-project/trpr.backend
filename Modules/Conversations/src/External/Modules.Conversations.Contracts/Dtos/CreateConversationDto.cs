using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Conversations.Contracts.Dtos
{
    public class CreateConversationDto
    {
        public Guid userId { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public List<Guid> ParticipantUserIds { get; set; } = [];
    }
}