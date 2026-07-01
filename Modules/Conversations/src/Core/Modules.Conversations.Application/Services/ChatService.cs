using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Application.Dtos.Requests;
using Modules.Conversations.Application.Dtos.Responses;
using Modules.Conversations.Application.Helpers;
using Modules.Conversations.Application.Interfaces;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Application.Services;

public class ChatService(
    RepositoryFactory repositoryFactory,
    INotificationSender notificationSender,
    IUnitOfWork unitOfWork)
{
    public async Task<MessageListItemDto> SendMessage(Guid userId, Guid conversationId, SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            var isConversationParticipant = await repositoryFactory
                .Repository<ConversationParticipant>()
                .GetQueryable()
                .AnyAsync(x =>
                    x.ConversationId == conversationId &&
                    x.UserId == userId,
                    cancellationToken);

            if (!isConversationParticipant)
                throw new NotFoundException("Conversation.NotFound");

            var conversation = await repositoryFactory.Repository<Conversation>()
                .GetForUpdateAsync(conversationId) ?? throw new NotFoundException("Conversation.NotFound");

            var messageEntity = Message.Create(conversation, userId, request.MessageContent);
            repositoryFactory.Repository<Message>().Add(messageEntity);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await notificationSender.SendMessageAsync(messageEntity, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new MessageListItemDto
            {
                Id = messageEntity.Id,
                Content = messageEntity.Content,
                SentAtUtc = messageEntity.SentAtUtc,
                SequenceNumber = messageEntity.SequenceNumber,
                SenderUserId = messageEntity.SenderUserId
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ConversationCursorPageDto> GetConversationsAsync(Guid userId, GetConversationsQueryDto query, CancellationToken cancellationToken = default)
    {
        var limit = Math.Clamp(query.Limit, 1, 100);

        var conversationsQuery = repositoryFactory.Repository<ConversationParticipant>()
            .GetQueryable()
            .Where(cp => cp.UserId == userId)
            .Select(cp => new
            {
                cp.Conversation.Id,
                cp.Conversation.Title,
                cp.Conversation.ImageUrl,
                cp.JoinedAtUtc,
                LastMessageId = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.Id)
                    .FirstOrDefault(),
                LastMessageSequence = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.SequenceNumber)
                    .FirstOrDefault(),
                LastMessageText = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.Content)
                    .FirstOrDefault(),
                LastMessageSenderId = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.SenderUserId)
                    .FirstOrDefault(),
                LastMessageSentAt = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.SentAtUtc)
                    .FirstOrDefault(),
                TotalUnread = cp.Conversation.Messages.Count(m => m.SentAtUtc > cp.JoinedAtUtc),
                LastRead = cp.Conversation.Messages
                    .Where(m => m.SentAtUtc <= cp.JoinedAtUtc)
                    .Select(m => m.SequenceNumber)
                    .DefaultIfEmpty(0)
                    .Max()
            });

        if (query.Cursor.HasValue)
            conversationsQuery = conversationsQuery.Where(c => c.Id < query.Cursor.Value);

        var pageItems = await conversationsQuery
            .OrderByDescending(c => c.Id)
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = pageItems.Count > limit;
        if (hasMore)
            pageItems.RemoveAt(pageItems.Count - 1);

        var items = pageItems.Select(item => new ConversationSummaryResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            LastMessage = item.LastMessageId == default ? null : new ConversationLastMessageDto
            {
                Id = item.LastMessageId.ToString(),
                SequenceNumber = item.LastMessageSequence,
                Text = item.LastMessageText ?? string.Empty,
                SenderId = item.LastMessageSenderId,
                SentAt = item.LastMessageSentAt
            },
            LastReadSequence = item.LastRead,
            UnreadCount = item.TotalUnread,
        })
        .ToList();

        Guid? nextCursor = null;
        if (hasMore && items.Any())
        {
            var last = items.Last();
            nextCursor = last.Id;
        }

        return new ConversationCursorPageDto
        {
            Items = items,
            NextCursor = nextCursor
        };
    }

    public async Task<ConversationDetailsResponseDto> GetConversationDetailsAsync(Guid userId, Guid conversationId, CancellationToken cancellationToken = default)
    {
        var conversation = await repositoryFactory.Repository<ConversationParticipant>()
            .GetQueryable()
            .Where(cp => cp.UserId == userId && cp.ConversationId == conversationId)
            .Select(cp => new
            {
                cp.Conversation.Id,
                cp.Conversation.Title,
                cp.Conversation.ImageUrl,
                cp.JoinedAtUtc,
                LastMessageId = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.Id)
                    .FirstOrDefault(),
                LastMessageSequence = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.SequenceNumber)
                    .FirstOrDefault(),
                LastMessageText = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.Content)
                    .FirstOrDefault(),
                LastMessageSenderId = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.SenderUserId)
                    .FirstOrDefault(),
                LastMessageSentAt = cp.Conversation.Messages
                    .OrderByDescending(m => m.SequenceNumber)
                    .Select(m => m.SentAtUtc)
                    .FirstOrDefault(),
                TotalUnread = cp.Conversation.Messages.Count(m => m.SentAtUtc > cp.JoinedAtUtc),
                LastRead = cp.Conversation.Messages
                    .Where(m => m.SentAtUtc <= cp.JoinedAtUtc)
                    .Select(m => m.SequenceNumber)
                    .DefaultIfEmpty(0)
                    .Max()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (conversation == null)
            throw new NotFoundException("Conversation.NotFound");

        return new ConversationDetailsResponseDto
        {
            Id = conversation.Id.ToString(),
            Title = conversation.Title,
            ImageUrl = conversation.ImageUrl,
            LastMessage = conversation.LastMessageId == default ? null : new ConversationLastMessageDto
            {
                Id = conversation.LastMessageId.ToString(),
                SequenceNumber = conversation.LastMessageSequence,
                Text = conversation.LastMessageText ?? string.Empty,
                SenderId = conversation.LastMessageSenderId,
                SentAt = conversation.LastMessageSentAt
            },
            LastReadSequence = conversation.LastRead,
            UnreadCount = conversation.TotalUnread
        };
    }

    public async Task<MessageCursorPageDto> GetConversationMessagesAsync(Guid userId, Guid conversationId, GetConversationMessagesQueryDto query, CancellationToken cancellationToken = default)
    {
        var isParticipant = await repositoryFactory.Repository<ConversationParticipant>()
            .GetQueryable()
            .AnyAsync(x => x.ConversationId == conversationId && x.UserId == userId, cancellationToken);

        if (!isParticipant)
            throw new NotFoundException("Conversation.NotFound");

        var limit = Math.Clamp(query.Limit, 1, 200);
        var messagesQuery = repositoryFactory.Repository<Message>().GetQueryable()
            .Where(m => m.ConversationId == conversationId);

        if (query.AfterSequence.HasValue)
        {
            messagesQuery = messagesQuery.Where(m => m.SequenceNumber > query.AfterSequence.Value);
        }

        var messageItems = await messagesQuery
            .OrderBy(m => m.SequenceNumber)
            .Select(m => new MessageListItemDto
            {
                Id = m.Id,
                SequenceNumber = m.SequenceNumber,
                Content = m.Content,
                SentAtUtc = m.SentAtUtc,
                SenderUserId = m.SenderUserId
            })
            .Take(limit + 1)
            .ToListAsync(cancellationToken);

        var hasMore = messageItems.Count > limit;
        if (hasMore)
            messageItems.RemoveAt(messageItems.Count - 1);

        int? nextCursor = hasMore ? messageItems.Last().SequenceNumber : null;

        return new MessageCursorPageDto
        {
            Items = messageItems,
            NextCursor = nextCursor
        };
    }
}
