using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Services;
using Modules.Conversations.Contracts.Contracts;
using Modules.Conversations.Contracts.Dtos;

namespace Modules.Conversations.Infrastructure;

public class ConversationContract(
    ChatService chatService) : IConversationsContract
{
    public async Task CreateConversation(CreateConversationDto request)
    {
        await chatService.CreateConversation(request.userId, new CreateConversationRequestDto
        {
            Title = request.Title,
            ImageUrl = request.ImageUrl,
            ParticipantUserIds = request.ParticipantUserIds
        });
    }
}
