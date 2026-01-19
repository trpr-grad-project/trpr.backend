using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authentication;
using Common.Presentation.AuthenticationHandlers;

namespace Common.Presentation;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddCommonPresentation(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddControllers().AddApplicationParts(assemblies);
        services.AddAuthentication(SchemaDefaults.ForwardedClaimsSchema)
        .AddScheme<AuthenticationSchemeOptions, ForwardedClaimsAuthenticationHandler>(SchemaDefaults.ForwardedClaimsSchema, null);
        services.AddAuthorization();
        return services;
    }
}
