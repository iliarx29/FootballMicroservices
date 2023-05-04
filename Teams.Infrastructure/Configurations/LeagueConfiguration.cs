using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teams.Infrastructure.Entities;

namespace Teams.Infrastructure.Configurations
{
    public class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).IsRequired();
            builder.Property(l => l.Code).IsRequired();
            builder.Property(l => l.CountryName).IsRequired();

            builder.HasMany(l => l.Teams)
                .WithOne(t => t.League)
                .HasForeignKey(t => t.LeagueId);
        }
    }
}