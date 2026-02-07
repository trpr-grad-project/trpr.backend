using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Infrastructure.Data;
using Modules.Users.Infrastructure.Data;

namespace Api.Extensions;

public static class MigrationsExtension
{
    public static void AddMigrations(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider
            .GetRequiredService<UsersDbContext>();
        var notificationsDbContext = scope.ServiceProvider
            .GetRequiredService<NotificationDbContext>();
        usersDbContext.Database.Migrate();
        notificationsDbContext.Database.Migrate();
    }
}
