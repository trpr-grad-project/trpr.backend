using Microsoft.Extensions.Logging;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Abstractions;

namespace Modules.Users.Application.Events.Handlers;

public class UserCreatedDomainEventHandler(IGenericRepository<User, Guid> userGenericRepository, ILogger<UserCreatedDomainEventHandler> logger) : DomainEventHandler<UserCreatedDomainEvent>
{
    public override async Task HandleAsync(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var user = await userGenericRepository.GetById(domainEvent.UserId);
        if (user == null)
        {
            logger.LogError("User with ID {UserId} not found for UserCreatedDomainEvent", domainEvent.UserId);
            throw new InvalidOperationException($"User with ID {domainEvent.UserId} not found.");
        }
        logger.LogInformation("User created with ID: {UserId}, Identifier: {Identifier}", domainEvent.UserId, user.UserName);
    }
}
