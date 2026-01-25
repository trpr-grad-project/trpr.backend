namespace Common.Application.EventBus
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public Guid Id { get; init; }
        public DateTime CreatedOnUtc { get; init; }
    }
}