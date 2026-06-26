using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs
{
    public class SupportRequestConfig : IEntityTypeConfiguration<SupportRequest>
    {
        public void Configure(EntityTypeBuilder<SupportRequest> builder)
        {
            builder.ToTable("SupportRequests");

            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Subject)
                .IsRequired();

            builder.Property(sr => sr.Description)
                .IsRequired();


            builder.Property(sr => sr.Status)
                .IsRequired();
                

            builder.HasOne(sr => sr.user)
                .WithMany()
                .HasForeignKey(sr => sr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(sr => sr.Subject); 
            builder.HasIndex(sr => sr.Status);
        }
    }
}
