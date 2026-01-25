using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class MigrationsExtension
{
    public static void AddMigrations(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider
            .GetRequiredService<Modules.Users.Persistence.Data.AppDbContext>();
        var notificationsDbContext = scope.ServiceProvider
            .GetRequiredService<Modules.Notifications.Persistence.Data.AppDbContext>();
        usersDbContext.Database.Migrate();
        notificationsDbContext.Database.Migrate();
    }
}
