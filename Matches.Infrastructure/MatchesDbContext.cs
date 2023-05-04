using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Matches.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Matches.Infrastructure;

public class MatchesDbContext : DbContext, IMatchesDbContext
{
    public MatchesDbContext(DbContextOptions<MatchesDbContext> options)
        : base(options)
    { }

    public DbSet<Match> Matches { get; set; }
    public DbSet<Season> Seasons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MatchConfiguration());

        modelBuilder.Entity<Season>(s =>
        {
            s.HasData(
                new Season
                {
                    Id = Guid.NewGuid(),
                    Years = "2022/2023",
                    StartDate = new DateTime(2022, 8, 5, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = null,
                    LeagueId = new Guid("6e64b1e4-d662-4c04-8d67-0db65ead9eb7"),
                    TeamWinnerId = null
                });
        });
    }
}
