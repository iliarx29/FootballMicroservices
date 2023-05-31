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
        builder.Property(m => m.CompetitionId).IsRequired();
        builder.Property(m => m.Status).IsRequired();
        builder.Property(m => m.Stage).IsRequired();
        builder.Property(m => m.Season).IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>();

        builder.Property(m => m.Stage)
            .HasConversion<string>();

        builder.Property(m => m.Group)
            .HasConversion<string>();

        builder
            .HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
