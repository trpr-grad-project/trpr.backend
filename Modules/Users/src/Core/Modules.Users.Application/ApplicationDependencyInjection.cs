using Common.Application.DomainEvents.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application.Factories;
using Modules.Users.Application.Interfaces;
using Modules.Users.Application.Options;
using Modules.Users.Application.Services;
using Modules.Users.Contracts.Contracts;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        # region Options
        services.AddOptions<TokenExpirationInMinutesOption>()
            .BindConfiguration("Users:TokenExpirationInMinutes")
            .ValidateDataAnnotations();
        services.AddOptions<JwtOptions>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations();
        #endregion

        #region  factories
        services.AddScoped<TokenFactory>();
        #endregion

        #region  services
        services.AddScoped<UserService>();
        services.AddScoped<CompanyService>();
        services.AddScoped<AdminUserService>();
        services.AddScoped<GuideService>();
        services.AddScoped<SupportService>();
        services.AddScoped<ProfileManagementService>();
        services.AddScoped<OtpHandlerFactory>();
        services.AddKeyedScoped<ITokenHandler, ForgetPasswordOtpHandler>(TokenType.ForgetPasswordOtp);
        services.AddKeyedScoped<ITokenHandler, CreateUserOtpHandler>(TokenType.Otp);
        services.AddScoped<IUsersContract, UpdateUserRatingContract>();
        #endregion

        return services;
    }


}
