using System.Reflection;
using Common.Application.Correlation;
using Common.Application.DomainEvents;
using Common.Application.DomainEvents.Extensions;
using Common.Application.IntegrationEvents;
using Common.Application.IntegrationEvents.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddCommonApplication(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddDomainEventHandlers(x => x.AddAssemblies(assemblies));
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IIntegrationEventDispatcher,
        IntegrationEventDispatcher>();
        services.AddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();
        return services;
    }
}
