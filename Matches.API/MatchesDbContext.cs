using Matches.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Matches.API;

public class MatchesDbContext : DbContext
{
    public MatchesDbContext(DbContextOptions<MatchesDbContext> options)
        : base(options)
    { }

    public DbSet<Match> Matches => Set<Match>();
    public DbSet<League> Leagues => Set<League>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=matchesdb;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Match>(m =>
        {
            m.HasOne(m => m.League)
            .WithMany()
            .HasForeignKey(m => m.LeagueId);

            m.HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);

            m.HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        });

        modelBuilder.Entity<Team>(t =>
        {
            t.HasOne(t => t.Country)
            .WithMany()
            .HasForeignKey(t => t.CountryId);      
        });

        modelBuilder.Entity<League>(l =>
        {
            l.HasOne(l => l.Country)
            .WithMany()
            .HasForeignKey(l => l.CountryId);
        });
    }

}
