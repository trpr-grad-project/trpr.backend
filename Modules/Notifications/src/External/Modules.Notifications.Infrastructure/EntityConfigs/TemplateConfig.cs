using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class TemplateConfig : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Active);
        builder.HasMany(x => x.TemplateLangs)
            .WithOne(x => x.Template)
            .HasForeignKey(x => x.TemplateId);
    }
}
