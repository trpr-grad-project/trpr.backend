using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Infrastructure.EntityConfigs
{ 
    public class CompanyConfig: IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Identifier)
                .IsRequired();

            builder.HasIndex(c => c.Identifier)
                .IsUnique();

            builder.Property(c => c.Name)
                .IsRequired();

            builder.HasMany(c => c.Guides)
                .WithMany()
                .UsingEntity(j => j.ToTable("companyGuides"));
            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.Identifier);
        }
    }
    
}
