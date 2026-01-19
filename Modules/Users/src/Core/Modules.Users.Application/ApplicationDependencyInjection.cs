using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application.Events;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Services;
using Modules.Users.Application.Pipelines;

namespace Modules.Users.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register the domain event dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileManagementService, ProfileManagementService>();
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
