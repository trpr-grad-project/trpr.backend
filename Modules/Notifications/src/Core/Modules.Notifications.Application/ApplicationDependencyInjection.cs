using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Events;
using Modules.Notifications.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Notifications.Application.Pipelines;

namespace Modules.Notifications.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        #endregion

        #region  factories
        #endregion

        #region  services
        #endregion

        // Register the domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddDomainEventHandlers();
        return services;
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = AssemblyRefrence.Assembly
            .GetTypes()
            .Where(t =>
            !t.IsAbstract &&
            !t.IsInterface &&
            !t.IsGenericTypeDefinition &&
            t.IsAssignableTo(typeof(IDomainEventHandler<>)))
            .ToArray();


        foreach (Type domainEventHandler in domainEventHandlers)
        {
            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(x => x.IsGenericType)
                .GetGenericArguments()
                .Single();
            Type handlerInterface = typeof(IDomainEventHandler<>)
                .MakeGenericType(domainEvent);
            Type closedIdempotentHandler = typeof(OutboxIdempotentDomainEventHandlerDecorator<>)
                .MakeGenericType(domainEvent);
            services.TryAddScoped(handlerInterface, domainEventHandler);
            services.TryDecorate(handlerInterface, closedIdempotentHandler);
        }
    }
}
