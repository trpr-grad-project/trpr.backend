using Common.Application.Exceptions;
using Common.Domain.IntragationEvents;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Domain.Entities;
using Modules.Notifications.Application.Abstractions;

namespace Modules.Conversations.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork) : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task HandleAsync(UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository
            .GetFirstOrDefaultByFilter(x =>
                x.Id == integrationEvent.UserId);
        if (user != null)
            throw new ConflictException("User.Conflict");
        user = new User
        {
            Id = integrationEvent.UserId,
            FirstName = integrationEvent.FirstName,
            LastName = integrationEvent.LastName,
            Identifier = integrationEvent.UserName,
            AvatarUrl = null
        };
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}
