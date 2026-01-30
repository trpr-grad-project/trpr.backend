using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Users.Infrastructure.Inbox;

public class InboxConsumerMessageConfiguration : IEntityTypeConfiguration<InboxConsumerMessage>
{
    public void Configure(EntityTypeBuilder<InboxConsumerMessage> builder)
    {
        builder.HasKey(msg => new { msg.Id, msg.HandlerName });
    }
}
