using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application;
using Modules.Notifications.Infrastructure.Inbox;
using Modules.Notifications.Infrastructure.Outbox;
using Modules.Notifications.Presentation;
using Modules.Users.Contracts.IntegrationEvents;
using Rebus.Handlers;

namespace Modules.Notifications.Infrastructure
{
    public static class NotificationsModuleDependencyInject
    {
        public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPresentation();

            services
                .AddTransient<IHandleMessages<UserCreatedIntegrationEvent>, BaseIngtegrationEventHandler<UserCreatedIntegrationEvent>>();

            services.AddIntegrationEventHandlerDecorators(
                cfg =>
                    cfg.AddAssemblies(Presentation.AssemblyRefrence.Assembly)
                    .AddPipeline(typeof(InboxIdempotentIntegrationEventHandlerDecorator<>)));

            services.AddDomainEventHandlerDecorators(
                cfg =>
                    cfg.AddAssemblies(Application.AssemblyRefrence.Assembly)
                    .AddPipeline(typeof(OutboxIdempotentDomainEventHandlerDecorator<>)));
            return services;
        }
    }
}
