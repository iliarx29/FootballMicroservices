using Microsoft.EntityFrameworkCore;
using Teams.Infrastructure.Entities;

namespace Teams.Infrastructure;

public class TeamsDbContext : DbContext
{
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<League> Leagues => Set<League>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=teamsdb;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>(t =>
        {
            t.HasOne(t => t.League)
            .WithMany(l => l.Teams)
            .HasForeignKey(t => t.LeagueId);
        });
    }
}
