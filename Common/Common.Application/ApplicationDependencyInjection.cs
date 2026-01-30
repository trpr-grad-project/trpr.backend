using System.Reflection;
using Common.Application.Correlation;
using Common.Application.DomainEvents;
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
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();
        return services;
    }
}
