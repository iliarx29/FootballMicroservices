using Matches.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Matches.Infrastructure.Configurations;
public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.CountryName).IsRequired();
        builder.Property(p => p.DateOfBirth).IsRequired();

        builder.HasMany(p => p.HomeMatches)
         .WithMany(m => m.HomePlayers)
         .UsingEntity(j => j.ToTable("HomePlayersMatches"));

        builder.HasMany(p => p.AwayMatches)
         .WithMany(m => m.AwayPlayers)
         .UsingEntity(j => j.ToTable("AwayPlayersMatches"));

        builder.HasOne(p => p.Team)
            .WithMany()
            .HasForeignKey(p => p.TeamId);

        builder.Property(p => p.Position)
            .HasConversion<string>();

        builder.Ignore(p => p.AllMatches);
    }
}
