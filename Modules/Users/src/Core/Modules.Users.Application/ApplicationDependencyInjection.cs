using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application.Events;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Services;
using Modules.Users.Application.Pipelines;
using Modules.Users.Application.Options;
using Modules.Users.Application.Factories;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        services.AddOptions<TokenExpirationInMinutesOption>()
            .BindConfiguration("TokenExpirationInMinutes")
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
