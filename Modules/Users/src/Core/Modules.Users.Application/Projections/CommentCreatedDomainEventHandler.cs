using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain.Events;

namespace Modules.Users.Application.Projections
{
    public class CommentCreatedDomainEventHandler : IDomainEventHandler<CommentCreatedDomainEvent>
    {
        public Task HandleAsync(CommentCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}