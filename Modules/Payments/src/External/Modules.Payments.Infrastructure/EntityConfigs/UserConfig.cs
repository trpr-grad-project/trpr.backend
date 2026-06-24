using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Payments.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserName);
        builder.HasIndex(x => x.Email);
        builder.HasIndex(x => x.PhoneNumber);
    }
}
