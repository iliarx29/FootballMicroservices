using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Infrastructure;
using Teams.Infrastructure.Entities;

namespace Teams.Domain.Services;
public class CompetitionService : ICompetitionService
{
    private readonly TeamsDbContext _context;
    private readonly IMapper _mapper;

    public CompetitionService(TeamsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CompetitionResponse>> GetAllCompetitionsAsync()
    {
        var competitions = await _context.Competitions.AsNoTracking().ToListAsync();

        var competitionsResponse = _mapper.Map<List<CompetitionResponse>>(competitions);

        return competitionsResponse;
    }

    public async Task<CompetitionResponse> GetCompetitionByIdAsync(Guid id)
    {
        var competition = await _context.Competitions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (competition is null)
            throw new ArgumentNullException($"Competition with id: '{id}' doesn't exists.");

        var competitionResponse = _mapper.Map<CompetitionResponse>(competition);

        return competitionResponse;
    }

    public async Task<CompetitionResponse> AddCompetitionAsync(CompetitionRequest competitionRequest)
    {
        var competition = _mapper.Map<Competition>(competitionRequest);

        await _context.Competitions.AddAsync(competition);
        await _context.SaveChangesAsync();

        var competitionResponse = _mapper.Map<CompetitionResponse>(competition);

        return competitionResponse;
    }

    public async Task UpdateCompetitionAsync(Guid id, CompetitionRequest competitionRequest)
    {
        var competition = await _context.Competitions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (competition is null)
        {
            throw new ArgumentNullException($"League with given id:'{id}' doesn't exist.");
        }

        competition = _mapper.Map<Competition>(competitionRequest);
        competition.Id = id;

        _context.Competitions.Update(competition);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCompetitionAsync(Guid id)
    {
        var competition = await _context.Competitions.FirstOrDefaultAsync(x => x.Id == id);
        if (competition is null)
        {
            throw new ArgumentNullException($"Competition with given id:'{id}' doesn't exist.");
        }

        _context.Competitions.Remove(competition);
        await _context.SaveChangesAsync();
    }

    public async Task<CompetitionResponse> GetCompetitionWithTeams(Guid id)
    {
        var competition = await _context.Competitions.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == id);

        if(competition is null)
        {
            throw new ArgumentNullException($"League with given id:'{id}' doesn't exist.");
        }

        var competitionResponse = _mapper.Map<CompetitionResponse>(competition);

        return competitionResponse;
    }

    public async Task<int> AddTeamsToCompetition(Guid competitionId, List<Guid> teamIds)
    {
        var competition = await _context.Competitions.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == competitionId);

        if (competition is null)
        {
            throw new ArgumentNullException($"Competition with given id:'{competitionId}' doesn't exist.");
        }

        var teams = teamIds.Select(x => new Team { Id = x }).ToList();

        _context.AttachRange(teams);

        competition.Teams.AddRange(teams);

       var result = await _context.SaveChangesAsync();

        return result;

    }
}
