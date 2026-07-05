using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Infrastructure.EntityConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.JoinedTrips)
                .WithOne(tp => tp.User)
                .HasForeignKey(tp => tp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Bids)
                .WithOne(b => b.Guide)
                .HasForeignKey(b => b.GuideId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
