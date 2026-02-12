using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Trips.Application;
using Modules.Trips.Infrastructure.Inbox;
using Modules.Trips.Infrastructure.Outbox;
using Modules.Trips.Presentation;

namespace Modules.Trips.Infrastructure
{
    public static class TripsModuleDependencyInject
    {
        public static IServiceCollection AddTripsModule(this IServiceCollection services, IConfiguration configuration)
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
