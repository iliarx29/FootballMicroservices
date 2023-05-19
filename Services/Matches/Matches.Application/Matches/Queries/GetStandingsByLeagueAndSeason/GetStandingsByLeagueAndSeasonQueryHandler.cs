using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
using System.Linq;
using Matches.Application.Results;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Matches.Application.Matches.Queries.GetStandingsByLeagueId;
public class GetStandingsByLeagueAndSeasonQueryHandler : IRequestHandler<GetStandingsByLeagueAndSeasonQuery, Result<List<Ranking>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;

    public GetStandingsByLeagueAndSeasonQueryHandler(IMatchesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<Ranking>>> Handle(GetStandingsByLeagueAndSeasonQuery query, CancellationToken cancellationToken)
    {
        var standings = _context.Matches
                        .Where(x => x.LeagueId == query.LeagueId)
                        .Where(x => x.SeasonId == query.SeasonId)
                        .Where(x => x.Status == Status.Finished)
                        .Select(x => new MatchForTeam { TeamId = x.HomeTeamId, GoalsScored = x.HomeGoals, GoalsConceded = x.AwayGoals })
                .Concat(
                         _context.Matches
                        .Where(x => x.LeagueId == query.LeagueId)
                        .Where(x => x.SeasonId == query.SeasonId)
                        .Where(x => x.Status == Status.Finished)
                        .Select(x => new MatchForTeam { TeamId = x.AwayTeamId, GoalsScored = x.AwayGoals, GoalsConceded = x.HomeGoals }))
        .GroupBy(x => x.TeamId)
        .ProjectTo<Ranking>(_mapper.ConfigurationProvider)
        .ToList();

        //.GroupBy(x => x.TeamId)
        //.Select(teams => new Ranking
        //{
        //    TeamId = teams.Key,
        //    Played = teams.Count(),
        //    Wins = teams.Count(x => x.GoalsScored > x.GoalsConceded),
        //    Loses = teams.Count(x => x.GoalsScored < x.GoalsConceded),
        //    Draws = teams.Count(x => x.GoalsScored == x.GoalsConceded),
        //    GoalsScored = (int)teams.Sum(x => x.GoalsScored),
        //    GoalsConceded = (int)teams.Sum(x => x.GoalsConceded)
        //}).ToListAsync();

        standings = standings.OrderByDescending(x => x.Points).ThenByDescending(x => x.GoalsDiff).ToList();

        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Position = i + 1;
        }

        return Result<List<Ranking>>.Success(standings);
    }
}

public class MatchForTeam
{
    public Guid TeamId { get; set; }
    public int? GoalsScored { get; set; }
    public int? GoalsConceded { get; set; }
}