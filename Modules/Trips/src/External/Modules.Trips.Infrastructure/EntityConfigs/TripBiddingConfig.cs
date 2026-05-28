using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class TripBiddingConfig : IEntityTypeConfiguration<TripBidding>
{
    public void Configure(EntityTypeBuilder<TripBidding> builder)
    {
        builder.HasKey(tb => tb.Id);

        builder
            .HasIndex(
                tb => new
                {
                    tb.TripId,
                    tb.GuideId
                })
            .IsUnique();

        builder.HasOne(tb => tb.Trip)
            .WithMany(t => t.Bids)
            .HasForeignKey(tb => tb.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tb => tb.Guide)
            .WithMany(u => u.Bids)
            .HasForeignKey(tb => tb.GuideId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
