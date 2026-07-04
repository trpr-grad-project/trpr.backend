using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Conversations.Application.Dtos.Responses
{
    public class MessageResponseDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? SenderUserId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}