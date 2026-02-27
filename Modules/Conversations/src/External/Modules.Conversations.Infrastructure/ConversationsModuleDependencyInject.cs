using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents.Extensions;
using Common.Domain.IntragationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Conversations.Application;
using Modules.Conversations.Infrastructure.Inbox;
using Modules.Conversations.Infrastructure.Outbox;
using Modules.Conversations.Presentation;
using Modules.Notifications.Infrastructure.Inbox;
using Rebus.Handlers;

namespace Modules.Conversations.Infrastructure
{
    public static class ConversationsModuleDependencyInject
    {
        public static IServiceCollection AddConversationsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPresentation();

            #region Integration Events Subscription
            services
                .AddTransient<IHandleMessages<UserCreatedIntegrationEvent>, BaseIngtegrationEventHandler<UserCreatedIntegrationEvent>>();
            #endregion

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
