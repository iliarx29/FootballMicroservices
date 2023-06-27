using Microsoft.EntityFrameworkCore;
using Teams.Infrastructure.Entities;
using Teams.Infrastructure.Repositories.Interfaces;

namespace Teams.Infrastructure.Repositories.Implementations;
public class TeamsRepository : ITeamsRepository
{
    private readonly TeamsDbContext _context;

    public TeamsRepository(TeamsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams.AsNoTracking().ToListAsync();

        return teams;
    }

    public async Task<Team?> GetTeamByIdAsync(Guid id)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return team;
    }
    public async Task AddTeamAsync(Team team)
    {
        await _context.Teams.AddAsync(team);
    }

    public async Task AddRange(List<Team> teams)
    {
        await _context.AddRangeAsync(teams);
    }

    public void UpdateTeam(Team team)
    {
        _context.Teams.Update(team);
    }

    public void DeleteTeam(Team team)
    {
        _context.Teams.Remove(team);
    }

    public Task<int> ImportTeams()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCount()
    {
        var count = await _context.Teams.CountAsync();

        return count;
    }

}
