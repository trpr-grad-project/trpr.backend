using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Modules.Payments.Application.Abstractions;
using Rebus.Handlers;

namespace Modules.Payments.Presentation
{
    public static class PresentationDependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            return services;
        }

    }
}