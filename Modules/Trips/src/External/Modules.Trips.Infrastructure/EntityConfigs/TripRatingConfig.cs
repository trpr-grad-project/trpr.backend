using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs
{
    public class TripRatingConfig : IEntityTypeConfiguration<TripRating>
    {
        public void Configure(EntityTypeBuilder<TripRating> builder)
        {
            builder.HasKey(tr => tr.Id);

            builder.HasOne(tr => tr.Trip)
                .WithMany()
                .HasForeignKey(tr => tr.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tr => tr.Reviewer)
                .WithMany()
                .HasForeignKey(tr => tr.ReviewerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint: one review per reviewer per trip
            builder.HasIndex(tr => new { tr.TripId, tr.ReviewerId })
                .IsUnique();

            
        }
    }
}
