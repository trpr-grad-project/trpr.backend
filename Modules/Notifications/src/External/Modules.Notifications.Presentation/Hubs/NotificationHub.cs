using System.Text.Json;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Mappers;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Presentation.Hubs;

[Authorize]
public class NotificationHub(RepositoryFactory repositoryFactory) : Hub
{
    public Guid UserId => Context.User!.GetUserId();
    public async override Task OnConnectedAsync()
    {
        var notification = await repositoryFactory
            .Repository<Notification>()
            .GetQueryable()
            .Where(x => x.UserId == UserId)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();
        var notificationDto = notification?.ToResponseDto();
        await Groups.AddToGroupAsync(Context.ConnectionId, UserId.ToString());
        await Clients.Caller.SendAsync("ReceiveNotification", notificationDto);
        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
