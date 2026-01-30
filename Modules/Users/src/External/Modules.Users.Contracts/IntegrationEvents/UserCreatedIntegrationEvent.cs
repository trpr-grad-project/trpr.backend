

using Common.Domain;

namespace Modules.Users.Contracts.IntegrationEvents;

public class UserCreatedIntegrationEvent(Guid Id, DateTime CreatedOnUtc) : IntegrationEvent(Id, CreatedOnUtc)
{
}
