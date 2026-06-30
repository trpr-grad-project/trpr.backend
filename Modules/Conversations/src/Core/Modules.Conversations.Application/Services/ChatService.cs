using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Application.Services;

public class ChatService(
    RepositoryFactory repositoryFactory,
    INotificationSender notificationSender,
    IUnitOfWork unitOfWork)
{
    public async Task<MessageResponseDto> SendMessage(Guid userId, Guid conversationId, SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        var conversation = await repositoryFactory.Repository<ConversationParticipant>()
            .GetQueryable().Where(x =>
                x.Id == conversationId && x.UserId == userId)
            .Include(x => x.Conversation)
            .Select(x => x.Conversation)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Conversation.NotFound");

        // Check if the user is a participant of the conversation
        var participant = conversation.Participants.FirstOrDefault(p => p.UserId == userId)
            ?? throw new NotFoundException("Conversation.ParticipantNotFound");

        var messageEntity = Message.Create(conversation, userId, request.MessageContent);
        // Save the message to the database
        repositoryFactory.Repository<Message>().Add(messageEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        // Send the message to the recipients via SignalR
        await notificationSender.SendMessageAsync(messageEntity, cancellationToken);

        return new MessageResponseDto
        {
            Id = messageEntity.Id.ToString(),
            SenderUserId = messageEntity.SenderUserId,
            Content = messageEntity.Content,
            ConversationId = messageEntity.ConversationId
        };
    }
    public async Task<ICollection<MessageResponseDto>> GetRelayMessagesAsync(Guid userId, ICollection<LastConversationMessageDto> lastConversationsMessages, CancellationToken cancellationToken = default)
    {
        var conversations = repositoryFactory.Repository<ConversationParticipant>()
            .GetQueryable()
            .Where(x => x.UserId == userId)
            .Include(x => x.Conversation)
            .Select(x => x.Conversation);

        var lastMessageConversation = lastConversationsMessages
            .ToDictionary(x => x.ConversationId, x => x.LastMessageId);

        var messages = new List<Message>();
        foreach (var conversation in conversations)
        {
            var convoMessagesQuery = repositoryFactory
                .Repository<Message>()
                .GetQueryable()
                .Where(m => m.ConversationId == conversation.Id);

            if (lastMessageConversation.TryGetValue(conversation.Id, out Guid lastMessageId))
            {
                convoMessagesQuery = convoMessagesQuery
                    .Where(x => x.Id > lastMessageId)
                    .OrderBy(m => m.SentAtUtc);
            }

            var convoMessages = await convoMessagesQuery.ToListAsync(cancellationToken);
            messages.AddRange(convoMessages);
        }

        return [.. messages.Select(messageEntity => new MessageResponseDto
        {
            Id = messageEntity.Id.ToString(),
            SenderUserId = messageEntity.SenderUserId,
            Content = messageEntity.Content,
            ConversationId = messageEntity.ConversationId
        })];
    }

}
