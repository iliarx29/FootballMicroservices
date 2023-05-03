using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Infrastructure;
using Teams.Infrastructure.Entities;

namespace Teams.Domain.Services;
public class TeamService : ITeamService
{
    private readonly TeamsDbContext _context;
    private readonly IMapper _mapper;

    public TeamService(TeamsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<TeamResponse>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams.AsNoTracking().ToListAsync();

        var teamsResponse = _mapper.Map<List<TeamResponse>>(teams);

        return teamsResponse;
    }

    public async Task<TeamResponse> GetTeamByIdAsync(Guid id)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (team is null)
            throw new ArgumentNullException($"Team with id: '{id}' doesn't exists.");

        var teamResponse = _mapper.Map<TeamResponse>(team);

        return teamResponse;
    }

    public async Task<TeamResponse> AddTeamAsync(TeamRequest teamRequest)
    {
        var team = _mapper.Map<Team>(teamRequest);

        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        var teamResponse = _mapper.Map<TeamResponse>(team);

        return teamResponse;
    }

    public async Task UpdateTeamAsync(Guid id, TeamRequest teamRequest)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (team is null)
        {
            throw new ArgumentNullException($"Team with given id:'{id}' doesn't exist.");
        }

        team = _mapper.Map<Team>(teamRequest);
        team.Id = id;

        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
        if (team is null)
        {
            throw new ArgumentNullException($"Team with given id:'{id}' doesn't exist.");
        }

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
    }
}
