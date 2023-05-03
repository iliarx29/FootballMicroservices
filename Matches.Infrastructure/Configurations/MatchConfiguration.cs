using Matches.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Matches.Infrastructure.Configurations;
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.HomeTeamId).IsRequired();
        builder.Property(m => m.AwayTeamId).IsRequired();
        builder.Property(m => m.LeagueId).IsRequired();
        builder.Property(m => m.SeasonId).IsRequired();
        builder.Property(m => m.Round).IsRequired();

        builder.HasOne(m => m.Season)
            .WithMany()
            .HasForeignKey(m => m.SeasonId);

        builder.Property(m => m.Status)
            .HasConversion<string>();
    }
}
