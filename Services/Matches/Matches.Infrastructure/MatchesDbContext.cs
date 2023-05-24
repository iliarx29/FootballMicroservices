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
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MatchConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
    }
}
