using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Interfaces
{
    public interface INotificationHubSender
    {
        public Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken = default);
    }
}