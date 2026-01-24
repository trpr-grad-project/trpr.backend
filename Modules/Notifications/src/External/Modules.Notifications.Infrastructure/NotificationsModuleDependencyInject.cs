using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application;
using Modules.Notifications.Persistence;
using Modules.Notifications.Presentation;

namespace Modules.Notifications.Infrastructure
{
    public static class NotificationsModuleDependencyInject
    {
        public static IServiceCollection AddNotificationsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPersistence(configuration);
            services.AddPresentation();
            return services;
        }
    }
}
