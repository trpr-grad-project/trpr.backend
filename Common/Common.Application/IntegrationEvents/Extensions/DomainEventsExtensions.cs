using Common.Application.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Abstractions;

namespace Common.Application.IntegrationEvents.Extensions
{
    public static class IntegrationEventsExtensions
    {
        public static void AddIntegrationEventHandlerDecorators(this IServiceCollection services, Action<IntegrationEventConfiguration> configure)
        {
            var config = new IntegrationEventConfiguration();
            configure(config);

            List<Type> integrationEventHandlers = [];
            foreach (var assembly in config.Assemblies.Assemblies)
            {
                var assemblyIntegrationEventHandlers = assembly
                   .GetTypes()
                   .Where(t =>
                   !t.IsAbstract &&
                   !t.IsInterface &&
                   !t.IsGenericTypeDefinition &&
                   t.IsAssignableTo(typeof(IIntegrationEventHandler<>)))
                   .ToList();
                integrationEventHandlers.AddRange(assemblyIntegrationEventHandlers);
            }

            foreach (Type integrationEventHandler in integrationEventHandlers)
            {
                Type integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(x => x.IsGenericType)
                    .GetGenericArguments()
                    .Single();
                Type handlerInterface = typeof(IIntegrationEventHandler<>)
                    .MakeGenericType(integrationEvent);
                foreach (var decorator in config.Decorators.Decorators)
                {
                    services.TryDecorate(
                        handlerInterface,
                        decorator.MakeGenericType(integrationEvent));
                }
            }
        }
        public static void AddIntegrationEventHandlers(this IServiceCollection services, Action<AssemblyConfiguration> configure)
        {
            var pipeline = new AssemblyConfiguration();
            configure(pipeline);

            List<Type> integrationEventHandlers = [];
            foreach (var assembly in pipeline.Assemblies)
            {
                var assemblyIntegrationEventHandlers = assembly
                   .GetTypes()
                   .Where(t =>
                   !t.IsAbstract &&
                   !t.IsInterface &&
                   !t.IsGenericTypeDefinition &&
                   t.IsAssignableTo(typeof(IIntegrationEventHandler<>)))
                   .ToList();
                integrationEventHandlers.AddRange(assemblyIntegrationEventHandlers);
            }

            foreach (Type integrationEventHandler in integrationEventHandlers)
            {
                Type integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(x => x.IsGenericType)
                    .GetGenericArguments()
                    .Single();
                Type handlerInterface = typeof(IIntegrationEventHandler<>)
                    .MakeGenericType(integrationEvent);

            }
        }
    }
}