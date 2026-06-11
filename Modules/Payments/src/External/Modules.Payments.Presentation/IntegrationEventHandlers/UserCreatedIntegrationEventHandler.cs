using Common.Application.IntegrationEvents;
using Common.Domain.IntragationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Payments.Application.Abstractions;
using Modules.Payments.Domain.Entities;

namespace Modules.Payments.Presentation.IntegrationEventHandlers;

public class UserCreatedIntegrationEventHandler(
    ILogger<UserCreatedIntegrationEventHandler> logger,
    IRepository<User> repo,
    IUnitOfWork unitOfWork) : IntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async override Task HandleAsync(UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating user with username : {userName} and id : {userId}", integrationEvent.UserName, integrationEvent.UserId);

        User? user = await repo.GetQueryable().FirstOrDefaultAsync(x => x.UserName == integrationEvent.UserName);
        if (user != null)
        {
            logger.LogInformation("User with username : {userName} already exist skipped creating the user", integrationEvent.UserName);
            return;
        }
        user = User.Create(integrationEvent.UserId, integrationEvent.UserName, integrationEvent.FirstName, integrationEvent.LastName);
        repo.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Created user with username : {userName} and id : {userId}", integrationEvent.UserName, integrationEvent.UserId);
    }

}
