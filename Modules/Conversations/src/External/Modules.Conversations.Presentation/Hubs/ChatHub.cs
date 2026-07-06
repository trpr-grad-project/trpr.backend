using System.Text.Json;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Presentation.Hubs;

[Authorize]
public class ChatHub(RepositoryFactory repositoryFactory) : Hub
{
    public Guid UserId => Context.User!.GetUserId();
    public async override Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", $"user id : {UserId}");
        var conversations = (await repositoryFactory
            .Repository<ConversationParticipant>()
            .GetByExpWhereAsync(x => x.UserId == UserId))
            .Select(x => x.ConversationId.ToString())
            .Distinct();

        foreach (var conversationId in conversations)
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

        await base.OnConnectedAsync();
    }

    public async Task ListenToConversation(Guid conversationId)
    {
        // Optional: verify the user belongs to this conversation
        var isParticipant = await repositoryFactory
            .Repository<ConversationParticipant>()
            .GetQueryable()
            .AnyAsync(x =>
                x.ConversationId == conversationId &&
                x.UserId == UserId);

        if (!isParticipant)
            throw new HubException("You are not a participant in this conversation.");

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", $"user id : {UserId}");
        await base.OnDisconnectedAsync(exception);
    }
}

