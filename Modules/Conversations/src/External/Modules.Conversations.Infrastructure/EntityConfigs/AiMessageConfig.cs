using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Conversations.Domain.Entities;

namespace Modules.Conversations.Infrastructure.EntityConfigs;

public class AiMessageConfig : IEntityTypeConfiguration<AiMessage>
{
    public void Configure(EntityTypeBuilder<AiMessage> builder)
    {
        builder.HasKey(am => am.Id);

        builder.Property(am => am.Contnet)
            .IsRequired();

        builder.Property(am => am.CreatedOnUtc)
            .IsRequired();

        builder.HasIndex(am => am.ConversationId);
        builder
            .HasOne(am => am.AiConversation)
            .WithMany(ac => ac.Messages)
            .HasForeignKey(am => am.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
