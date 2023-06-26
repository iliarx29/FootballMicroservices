using AutoMapper;
using AutoMapper.QueryableExtensions;
using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Matches.Infrastructure.Repositories;
public class MatchesRepository : IMatchesRepository
{
    private readonly MatchesDbContext _context;
    private readonly IMapper _mapper;

    public MatchesRepository(MatchesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Match>> GetMatchesAsync(CancellationToken cancellationToken = default)
    {
        var matches = await _context.Matches
            .AsNoTracking()
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<Match?> GetMatchByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var match = await _context.Matches
            .AsNoTracking()
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return match;
    }

    public async Task<List<Match>> GetH2HMatchesAsync(Guid homeTeamId, Guid awayTeamId, CancellationToken cancellationToken = default)
    {
        var h2hMatches = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .Where(x => x.Status == Status.Finished)
            .Where(x => (x.HomeTeamId == homeTeamId && x.AwayTeamId == awayTeamId)
                    || (x.AwayTeamId == homeTeamId && x.HomeTeamId == awayTeamId))
            .ToListAsync(cancellationToken);

        return h2hMatches;
    }

    public void AddMatch(Match match)
    {
        _context.Matches.Attach(match);
    }

    public void UpdateMatch(Match match)
    {
        _context.Matches.Update(match);
    }

    public void DeleteMatch(Match match)
    {
        _context.Matches.Remove(match);
    }

    public async Task<List<Match>> GetMatchesByCompetitionId(Guid id, CancellationToken cancellationToken = default)
    {
        var matches = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .Where(x => x.CompetitionId == id)
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<List<Match>> GetMatchesByTeamId(Guid id, CancellationToken cancellationToken = default)
    {
        var matches = await _context.Matches
               .Include(x => x.HomeTeam)
               .Include(x => x.AwayTeam)
               .AsNoTracking()
               .Where(x => x.HomeTeamId == id || x.AwayTeamId == id)
               .ToListAsync(cancellationToken);

        return matches;
    }

    public List<Ranking> GetStandingsByCompetitionAndSeason(Guid competitionId, string season)
    {
        var standings = _context.Matches
                                .Where(x => x.CompetitionId == competitionId)
                                .Where(x => x.Season == season)
                                .Where(x => x.Status == Status.Finished)
                                .Select(x => new MatchForTeam { TeamId = x.HomeTeamId, TeamName = x.HomeTeam.Name, GoalsScored = x.HomeGoals, GoalsConceded = x.AwayGoals })
                        .Concat(
                                 _context.Matches
                                 .Include(x => x.AwayTeam)
                                .Where(x => x.CompetitionId == competitionId)
                                .Where(x => x.Season == season)
                                .Where(x => x.Status == Status.Finished)
                                .Select(x => new MatchForTeam { TeamId = x.AwayTeamId, TeamName = x.AwayTeam.Name, GoalsScored = x.AwayGoals, GoalsConceded = x.HomeGoals }))
                .GroupBy(x => new GroupingObject { TeamId = x.TeamId, TeamName = x.TeamName })
                .ProjectTo<Ranking>(_mapper.ConfigurationProvider)
                .ToList();

        return standings;
    }
}
internal class GroupingObject
{
    public Guid TeamId { get; set; }
    public required string TeamName { get; set; }
}

internal class MatchForTeam
{
    public Guid TeamId { get; set; }
    public required string TeamName { get; set; }
    public int? GoalsScored { get; set; }
    public int? GoalsConceded { get; set; }
}