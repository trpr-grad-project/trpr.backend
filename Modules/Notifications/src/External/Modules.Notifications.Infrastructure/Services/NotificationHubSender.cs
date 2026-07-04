using Microsoft.AspNetCore.SignalR;
using Modules.Notifications.Application.Interfaces;
using Modules.Notifications.Application.Mappers;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Presentation.Hubs;

namespace Modules.Notifications.Infrastructure.Services
{
    public class NotificationHubSender(IHubContext<NotificationHub> hubContext) : INotificationHubSender
    {
        public async Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await hubContext
                .Clients
                .Group(
                    notification
                    .UserId
                    .ToString())
                .SendAsync("NotificationCreated", notification.ToResponseDto(), cancellationToken);
        }
    }
}