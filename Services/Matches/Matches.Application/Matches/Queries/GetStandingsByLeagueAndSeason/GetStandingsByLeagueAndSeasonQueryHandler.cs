using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
using System.Linq;
using Matches.Application.Results;

namespace Matches.Application.Matches.Queries.GetStandingsByLeagueId;
public class GetStandingsByLeagueAndSeasonQueryHandler : IRequestHandler<GetStandingsByLeagueAndSeasonQuery, Result<List<Ranking>>>
{
    private readonly IMatchesDbContext _context;

    public GetStandingsByLeagueAndSeasonQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Ranking>>> Handle(GetStandingsByLeagueAndSeasonQuery query, CancellationToken cancellationToken)
    {
        var standings = _context.Matches
                        .Where(x => x.LeagueId == query.LeagueId)
                        .Where(x => x.SeasonId == query.SeasonId)
                        .Where(x => x.Status == Status.Finished)
                        .Select(x => new { TeamId = x.HomeTeamId, GoalScored = x.HomeGoals, GoalConceded = x.AwayGoals })
                .Concat(
                         _context.Matches
                        .Where(x => x.LeagueId == query.LeagueId)
                        .Where(x => x.SeasonId == query.SeasonId)
                        .Where(x => x.Status == Status.Finished)
                        .Select(x => new { TeamId = x.AwayTeamId, GoalScored = x.AwayGoals, GoalConceded = x.HomeGoals }))
                .AsEnumerable()
                .GroupBy(x => x.TeamId)
                .Select(teams => new Ranking
                {
                    TeamId = teams.Key,
                    Played = teams.Count(),
                    Wins = teams.Count(x => x.GoalScored > x.GoalConceded),
                    Loses = teams.Count(x => x.GoalScored < x.GoalConceded),
                    Draws = teams.Count(x => x.GoalScored == x.GoalConceded),
                    GoalsScored = (int)teams.Sum(x => x.GoalScored),
                    GoalsConceded = (int)teams.Sum(x => x.GoalConceded)
                })
                .OrderByDescending(x => x.Points)
                    .ThenByDescending(x => x.GoalsDiff)
                .ToList();

        for(int i = 0; i < standings.Count; i++)
        {
            standings[i].Position = i + 1;
        }

        return Result<List<Ranking>>.Success(standings);
    }
}
