using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application;
using Modules.Users.Persistence;
using Modules.Users.Presentation;

namespace Modules.Users.Infrastructure
{
    public static class UsersModuleDependencyInject
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddPersistence(configuration);
            services.AddPresentation();
            return services; 
        }
    }
}
