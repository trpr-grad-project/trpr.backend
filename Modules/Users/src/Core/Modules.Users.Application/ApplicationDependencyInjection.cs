using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Services;
using Modules.Users.Application.Pipelines;
using Modules.Users.Application.Options;
using Modules.Users.Application.Factories;
using Modules.Users.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Common.Application.DomainEvents;
using Common.Domain;
using Common.Application.DomainEvents.Extensions;

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

        services.AddDomainEventHandlers(cfg =>
            cfg
                .AddAssembly(Application.AssemblyRefrence.Assembly)
        );
        services.AddDomainEventHandlerDecorators(cfg =>
            cfg
                .AddPipeline(typeof(OutboxIdempotentDomainEventHandlerDecorator<>))
                .AddAssembly(Application.AssemblyRefrence.Assembly));
        return services;
    }


}
