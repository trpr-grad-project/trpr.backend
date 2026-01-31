using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application;
using Modules.Users.Infrastructure.Inbox;
using Modules.Users.Infrastructure.Outbox;
using Modules.Users.Presentation;

namespace Modules.Users.Infrastructure
{
    public static class UsersModuleDependencyInject
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPresentation();

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
