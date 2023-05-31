using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teams.Domain.Interfaces;
using Teams.Domain.Models;
using Teams.Domain.Results;
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
    public async Task<Result<IEnumerable<CompetitionResponse>>> GetAllCompetitionsAsync()
    {
        var competitions = await _context.Competitions.AsNoTracking().ToListAsync();

        var competitionsResponse = _mapper.Map<List<CompetitionResponse>>(competitions);

        return Result<IEnumerable<CompetitionResponse>>.Success(competitionsResponse);
    }

    public async Task<Result<CompetitionResponse>> GetCompetitionByIdAsync(Guid id)
    {
        var competition = await _context.Competitions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (competition is null)
            return Result<CompetitionResponse>.Error(ErrorCode.NotFound, $"Competition with id: '{id}' doesn't exist.");

        var competitionResponse = _mapper.Map<CompetitionResponse>(competition);

        return Result<CompetitionResponse>.Success(competitionResponse);
    }

    public async Task<Result<CompetitionResponse>> AddCompetitionAsync(CompetitionRequest competitionRequest)
    {
        var competition = _mapper.Map<Competition>(competitionRequest);

        await _context.Competitions.AddAsync(competition);
        await _context.SaveChangesAsync();

        var competitionResponse = _mapper.Map<CompetitionResponse>(competition);

        return Result<CompetitionResponse>.Success(competitionResponse);
    }

    public async Task<Result> UpdateCompetitionAsync(Guid id, CompetitionRequest competitionRequest)
    {
        var competition = await _context.Competitions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (competition is null)
            return Result.Error(ErrorCode.NotFound, $"Competition with given id:'{id}' doesn't exist.");

        competition = _mapper.Map<Competition>(competitionRequest);
        competition.Id = id;

        _context.Competitions.Update(competition);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteCompetitionAsync(Guid id)
    {
        var competition = await _context.Competitions.FirstOrDefaultAsync(x => x.Id == id);
        if (competition is null)
            return Result.Error(ErrorCode.NotFound, $"Competition with given id:'{id}' doesn't exist.");

        _context.Competitions.Remove(competition);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<CompetitionResponse>> GetCompetitionWithTeams(Guid id)
    {
        var competition = await _context.Competitions.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == id);

        if (competition is null)
            return Result<CompetitionResponse>.Error(ErrorCode.NotFound, $"League with given id:'{id}' doesn't exist.");

        var competitionResponse = _mapper.Map<CompetitionResponse>(competition);

        return Result<CompetitionResponse>.Success(competitionResponse);
    }

    public async Task<Result<int>> AddTeamsToCompetition(Guid competitionId, List<Guid> teamsIds)
    {
        var competition = await _context.Competitions.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == competitionId);

        if (competition is null)
            return Result.Error(ErrorCode.NotFound, $"Competition with given id:'{competitionId}' doesn't exist.");

        var teams = teamsIds.Select(x => new Team { Id = x }).ToList();

        _context.AttachRange(teams);

        competition.Teams.AddRange(teams);

        var result = await _context.SaveChangesAsync();

        return Result<int>.Success(result);
    }

    public async Task<Result<int>> RemoveTeamsFromCompetition(Guid competitionId, List<Guid> teamsIds)
    {
        var competition = await _context.Competitions.Include(x => x.Teams).FirstOrDefaultAsync(x => x.Id == competitionId);

        if (competition is null)
            return Result.Error(ErrorCode.NotFound, $"Competition with given id:'{competitionId}' doesn't exist.");

        competition.Teams.RemoveRange(0, teamsIds.Count);

        var result = await _context.SaveChangesAsync();

        return Result<int>.Success(result);
    }
}
