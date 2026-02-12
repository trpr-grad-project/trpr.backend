using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Trips.Infrastructure.Outbox;

public class OutboxConsumerMessageConfiguration : IEntityTypeConfiguration<OutboxConsumerMessage>
{
    public void Configure(EntityTypeBuilder<OutboxConsumerMessage> builder)
    {
        builder.ToTable("outbox_consumer_messages");
        builder.HasKey(msg => new { msg.Id, msg.HandlerName });
    }
}
