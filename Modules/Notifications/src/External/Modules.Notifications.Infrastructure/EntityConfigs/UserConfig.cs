using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TripUpdates)
        .HasDefaultValue(true);
        builder.Property(x => x.Messages)
        .HasDefaultValue(true);
        builder.Property(x => x.Promotions)
        .HasDefaultValue(true);
    }
}
