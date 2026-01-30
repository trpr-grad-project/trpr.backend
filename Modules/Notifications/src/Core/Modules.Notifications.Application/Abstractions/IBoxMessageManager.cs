using Common.Domain;


namespace Modules.Notifications.Application.Abstractions
{
    public interface IBoxMessageManager
    {
        public Task InsertIntegrationEventToInBox(IntegrationEvent integrationEvents);
        public Task InsertDomainEventsToOutbox(DomainEvent domainEvents);
    }
}