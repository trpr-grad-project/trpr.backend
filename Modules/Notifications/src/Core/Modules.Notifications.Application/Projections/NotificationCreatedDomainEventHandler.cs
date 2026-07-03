using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.DomainEvents;
using Microsoft.AspNetCore.SignalR;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Mappers;
using Modules.Notifications.Domain.DomainEvents;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Projections
{
    public class NotificationCreatedDomainEventHandler(RepositoryFactory repositoryFactory, IHubContext hubContext) : IDomainEventHandler<NotificationCreatedDomainEvent>
    {
        public async Task HandleAsync(NotificationCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var notification = await repositoryFactory
                .Repository<Notification>()
                .GetFirstOrDefaultByFilter(x => x.Id == domainEvent.Id)
                ?? throw new InvalidOperationException($"Notification with Id {domainEvent.Id} not found.");

            await hubContext.Clients.User(notification.UserId.ToString())
                    .SendAsync("NotificationCreated", notification.ToResponseDto(), cancellationToken);

        }
    }
}