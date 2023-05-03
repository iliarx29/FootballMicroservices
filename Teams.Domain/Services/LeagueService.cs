using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Infrastructure;
using Teams.Infrastructure.Entities;

namespace Teams.Domain.Services;
public class LeagueService : ILeagueService
{
    private readonly TeamsDbContext _context;
    private readonly IMapper _mapper;

    public LeagueService(TeamsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<LeagueResponse>> GetAllLeaguesAsync()
    {
        var leagues = await _context.Leagues.AsNoTracking().ToListAsync();

        var leaguesResponse = _mapper.Map<List<LeagueResponse>>(leagues);

        return leaguesResponse;
    }

    public async Task<LeagueResponse> GetLeagueByIdAsync(Guid id)
    {
        var league = await _context.Leagues.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (league is null)
            throw new ArgumentNullException($"League with id: '{id}' doesn't exists.");

        var leagueResponse = _mapper.Map<LeagueResponse>(league);

        return leagueResponse;
    }

    public async Task<LeagueResponse> AddLeagueAsync(LeagueRequest leagueRequest)
    {
        var league = _mapper.Map<League>(leagueRequest);

        await _context.Leagues.AddAsync(league);
        await _context.SaveChangesAsync();

        var leagueResponse = _mapper.Map<LeagueResponse>(league);

        return leagueResponse;
    }

    public async Task UpdateLeagueAsync(Guid id, LeagueRequest leagueRequest)
    {
        var league = await _context.Leagues.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (league is null)
        {
            throw new ArgumentNullException($"League with given id:'{id}' doesn't exist.");
        }

        league = _mapper.Map<League>(leagueRequest);
        league.Id = id;

        _context.Leagues.Update(league);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLeagueAsync(Guid id)
    {
        var league = await _context.Leagues.FirstOrDefaultAsync(x => x.Id == id);
        if (league is null)
        {
            throw new ArgumentNullException($"League with given id:'{id}' doesn't exist.");
        }

        _context.Leagues.Remove(league);
        await _context.SaveChangesAsync();
    }

    public async Task<LeagueResponse> GetTeamsOfLeagueAsync(Guid id)
    {
        var league = await _context.Leagues.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == id);

        if(league is null)
        {
            throw new ArgumentNullException($"League with given id:'{id}' doesn't exist.");
        }

        var leagueResponse = _mapper.Map<LeagueResponse>(league);

        return leagueResponse;
    }
}
