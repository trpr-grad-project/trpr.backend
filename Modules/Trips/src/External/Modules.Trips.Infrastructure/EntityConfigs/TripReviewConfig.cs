using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class TripReviewConfig : IEntityTypeConfiguration<TripReview>
{
    public void Configure(EntityTypeBuilder<TripReview> builder)
    {
        builder.HasKey(tr => tr.Id);

        builder.HasOne(tr => tr.Trip)
            .WithMany()
            .HasForeignKey(tr => tr.TripId)
            .IsRequired();

        builder.HasOne(tr => tr.Reviewer)
            .WithMany()
            .HasForeignKey(tr => tr.ReviewerId)
            .IsRequired();

        builder.HasOne(tr => tr.Reviewee)
            .WithMany()
            .HasForeignKey(tr => tr.RevieweeId)
            .IsRequired();

        builder.HasIndex(tr => new { tr.TripId, tr.ReviewerId, tr.RevieweeId })
            .IsUnique();
    }
}
