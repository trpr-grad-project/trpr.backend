using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application;
using Modules.Notifications.Infrastructure.Inbox;
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
            return services;
        }
    }
}
