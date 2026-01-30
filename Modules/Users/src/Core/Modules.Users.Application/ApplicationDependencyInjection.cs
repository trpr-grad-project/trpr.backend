using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application.Events;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Services;
using Modules.Users.Application.Pipelines;
using Modules.Users.Application.Options;
using Modules.Users.Application.Factories;
using Modules.Users.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Modules.Users.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        services.AddOptions<TokenExpirationInMinutesOption>()
            .BindConfiguration("Users:TokenExpirationInMinutes")
            .ValidateDataAnnotations();
        #endregion

        #region  factories
        services.AddScoped<TokenFactory>();
        #endregion

        #region  services
        services.AddScoped<UserService>();
        services.AddScoped<ProfileManagementService>();
        services.AddScoped<OtpHandlerFactory>();
        services.AddKeyedScoped<ITokenHandler, ForgetPasswordOtpHandler>(TokenType.ForgetPasswordOtp);
        services.AddKeyedScoped<ITokenHandler, CreateUserOtpHandler>(TokenType.Otp);
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
