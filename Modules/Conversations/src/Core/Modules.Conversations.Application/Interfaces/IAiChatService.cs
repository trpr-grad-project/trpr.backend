using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;

namespace Modules.Conversations.Application.Interfaces
{
    public interface IAiChatService
    {
        public Task<MessageResponseDto> SendMessageAsync(
            Guid userId,
            SendAiPromptRequestDto request,
            CancellationToken cancellationToken = default);
    }
}