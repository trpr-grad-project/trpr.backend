using Microsoft.Extensions.DependencyInjection;
using Modules.Notifications.Application.Events;
using Modules.Notifications.Application.Pipelines;
using Modules.Notifications.Application.Abstractions;

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
        // Register domain event handlers
        services.Scan(scan => scan
            .FromAssemblies(AssemblyRefrence.Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        services.Decorate(typeof(IDomainEventHandler<>), typeof(LoggingDomainEventHandlerDecorator<>));
        services.Decorate(typeof(IDomainEventHandler<>), typeof(OutboxIdempotentDomainEventHandlerDecorator<>));
        return services;
    }
}
