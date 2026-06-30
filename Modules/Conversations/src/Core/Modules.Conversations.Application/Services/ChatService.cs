using System.Security.Cryptography.X509Certificates;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;
using Modules.Conversations.Domain.ValueObjects;

namespace Modules.Conversations.Application.Services;

public class ChatService(
    RepositoryFactory repositoryFactory,
    INotificationSender notificationSender,
    IUnitOfWork unitOfWork)
{
    public async Task<MessageResponseDto> StartDirectMessage(Guid userId, SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        // Check if the recipient user exists
        var recipientUser = await repositoryFactory.Repository<User>()
            .GetFirstOrDefaultByFilter(u => u.Id == request.RecipientId)
                ?? throw new NotFoundException("User.NotFound");

        // check if conversation already exists between the two users
        var existingConversation = await repositoryFactory.Repository<Conversation>()
            .GetQueryable()
            .Include(c => c.Participants)
            .Where(c =>
                c.Type == ConversationType.Direct &&
                c.Participants.Any(p => p.UserId == userId) &&
                c.Participants.Any(p => p.UserId == request.RecipientId))
            .FirstOrDefaultAsync(cancellationToken);

        if (existingConversation != null)
        {
            // If a conversation already exists, use it
            request.RecipientId = existingConversation.Id;
            return await SendMessage(userId, request, cancellationToken);
        }

        // Create a new conversation
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Type = ConversationType.Direct,
            CreateByUserId = userId,
        };

        repositoryFactory.Repository<Conversation>().Add(conversation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Add participants to the conversation
        var participants = new List<ConversationParticipant>
        {
            new() {
                ConversationId = conversation.Id,
                Conversation = conversation,
                UserId = userId,
            },
            new() {
                ConversationId = conversation.Id,
                Conversation = conversation,
                UserId = request.RecipientId,
            }
        };

        foreach (var participant in participants)
            repositoryFactory.Repository<ConversationParticipant>().Add(participant);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Set the recipient ID to the conversation ID for sending the first message
        request.RecipientId = conversation.Id;
        // Send the first message
        return await SendMessage(userId, request, cancellationToken);
    }
    public async Task<MessageResponseDto> SendMessage(Guid userId, SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(
            x => x.Id == userId
        ) ?? throw new NotFoundException("User.NotFound");
        // Check if the conversation exists and the user is a participant
        var conversation = await repositoryFactory.Repository<Conversation>()
            .GetQueryable().Where(x =>
                x.Id == request.RecipientId)
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Conversation.NotFound");

        // Check if the user is a participant of the conversation
        var participant = conversation.Participants.FirstOrDefault(p => p.UserId == userId)
            ?? throw new NotFoundException("Conversation.ParticipantNotFound");

        var messageEntity = Message.Create(conversation, user, request.MessageContent);
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
