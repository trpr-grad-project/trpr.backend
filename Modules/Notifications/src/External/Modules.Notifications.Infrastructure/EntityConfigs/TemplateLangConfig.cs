using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Infrastructure.EntityConfigs;

public class TemplateLangConfig : IEntityTypeConfiguration<TemplateLang>
{
    public void Configure(EntityTypeBuilder<TemplateLang> builder)
    {
        builder.HasKey(x => new { x.TemplateId, x.LangCode });
    }
}