using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Notifications.Domain.Entities.Inbox;

namespace Modules.Notifications.Persistence.EntityConfigs.Inbox;

public class InboxConsumerMessageConfiguration : IEntityTypeConfiguration<InboxConsumerMessage>
{
    public void Configure(EntityTypeBuilder<InboxConsumerMessage> builder)
    {
        builder.HasKey(msg => new { msg.Id, msg.HandlerName });
    }
}
