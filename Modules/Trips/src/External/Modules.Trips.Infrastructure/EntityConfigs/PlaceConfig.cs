using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs;

public class PlaceConfig : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.GovernorateId);
        builder.HasIndex(p => p.CategoryId);

        builder.Property(p => p.OsrmId)
            .HasMaxLength(100);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.Rating);
        builder.Property(p => p.AverageVisitTime);

        builder.Property(p => p.VisitCount)
            .HasDefaultValue(0);

        builder.Property(p => p.RateCount)
            .HasDefaultValue(0);

        builder.Property(p => p.Location)
            .HasColumnType("geometry (Point, 3857)");

        builder.HasIndex(p => p.Location)
            .HasMethod("GIST");

        builder
            .HasOne(p => p.Governorate)
            .WithMany()
            .HasForeignKey(p => p.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
