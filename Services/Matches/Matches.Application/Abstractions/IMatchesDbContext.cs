using Matches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Abstractions;
public interface IMatchesDbContext
{
    DbSet<Match> Matches { get; set; }
    DbSet<Season> Seasons { get; set; }
    DbSet<Player> Players { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
