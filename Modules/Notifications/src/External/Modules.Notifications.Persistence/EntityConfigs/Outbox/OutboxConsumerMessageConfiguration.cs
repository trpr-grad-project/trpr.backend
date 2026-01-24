using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities.Outbox;

namespace Modules.Notifications.Persistence.EntityConfigs.Outbox;

public class OutboxConsumerMessageConfiguration : IEntityTypeConfiguration<OutboxConsumerMessage>
{
    public void Configure(EntityTypeBuilder<OutboxConsumerMessage> builder)
    {
        builder.HasKey(msg => new { msg.Id, msg.HandlerName });
    }
}
