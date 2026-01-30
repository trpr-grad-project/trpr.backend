using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(c => c.UserName)
            .IsUnique();
        builder.Property(c => c.UserName)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(c => c.IdentityProviderId)
            .IsUnique();
        builder.Property(c => c.TwoFactorEnabled)
            .HasDefaultValue(false);
    }
}
