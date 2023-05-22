using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
using Matches.Domain.Entities.Enums;

namespace Matches.Application.Matches.Queries.GetStandingsByLeagueId;
public class GetStandingsByLeagueAndSeasonQueryHandler : IRequestHandler<GetStandingsByLeagueAndSeasonQuery, List<Ranking>>
{
    private readonly IMatchesDbContext _context;

    public GetStandingsByLeagueAndSeasonQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ranking>> Handle(GetStandingsByLeagueAndSeasonQuery query, CancellationToken cancellationToken)
    {
        var team = await _context.Matches.AsNoTracking()
                        .Where(x => x.CompetitionId == query.LeagueId && x.Season == query.Season && x.Status == Status.Finished)
                        .Select(x => new { x.HomeTeamId, x.HomeGoals, x.AwayGoals })
                .Concat(
                        _context.Matches.AsNoTracking()
                        .Where(x => x.CompetitionId == query.LeagueId && x.Season == query.Season && x.Status == Status.Finished)
                        .Select(x => new { HomeTeamId = x.AwayTeamId, HomeGoals = x.AwayGoals, AwayGoals = x.HomeGoals })
                        )
                .ToListAsync(cancellationToken);

        List<Ranking> standings = (from t in team
                                   group t by t.HomeTeamId into teams
                                   select new Ranking
                                   {
                                       TeamId = teams.Key,
                                       Played = teams.Count(),
                                       Wins = teams.Count(x => x.HomeGoals > x.AwayGoals),
                                       Loses = teams.Count(x => x.HomeGoals < x.AwayGoals),
                                       Draws = teams.Count(x => x.HomeGoals == x.AwayGoals),
                                       GoalsScored = (int)teams.Sum(x => x.HomeGoals),
                                       GoalsConceded = (int)teams.Sum(x => x.AwayGoals),
                                   })
                                   .OrderByDescending(x => x.Points)
                                   .ThenByDescending(x => x.GoalsDiff)
                                   .ToList();

        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Position = i + 1;
        }

        return standings;
    }
}
