using Microsoft.EntityFrameworkCore;
using Teams.Infrastructure.Configurations;
using Teams.Infrastructure.Entities;

namespace Teams.Infrastructure;

public class TeamsDbContext : DbContext
{
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Competition> Competitions => Set<Competition>();

    public TeamsDbContext(DbContextOptions<TeamsDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new CompetitionConfiguration());
    }
}
