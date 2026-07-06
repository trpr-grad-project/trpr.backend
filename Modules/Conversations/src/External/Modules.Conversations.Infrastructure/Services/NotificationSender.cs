using Microsoft.AspNetCore.SignalR;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Presentation.Hubs;

namespace Modules.Conversations.Infrastructure.Services;

public class NotificationSender(IHubContext<ChatHub> hubContext) : INotificationSender
{
    public async Task AddParticipantsToConversation(Conversation conversation, CancellationToken cancellationToken = default)
    {
        foreach (var participant in conversation.Participants)
            await hubContext.Clients.User(participant.UserId.ToString()).SendAsync("NewChatCreated", new
            {
                conversation.Id
            }, cancellationToken: cancellationToken);
    }

    public async Task SendMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients.Group(message.ConversationId.ToString()).SendAsync("ReceiveMessage", new
        MessageListItemDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderUserId = message.SenderUserId,
            Content = message.Content,
            SentAtUtc = message.SentAtUtc,
            SequenceNumber = message.SequenceNumber
        }, cancellationToken);
    }

}
