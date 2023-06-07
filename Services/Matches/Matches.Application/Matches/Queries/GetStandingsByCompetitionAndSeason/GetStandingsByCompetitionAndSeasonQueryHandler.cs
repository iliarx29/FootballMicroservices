using AutoMapper;
using AutoMapper.QueryableExtensions;
using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
public class GetStandingsByCompetitionAndSeasonQueryHandler : IRequestHandler<GetStandingsByCompetitionAndSeasonQuery, Result<List<Ranking>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;

    public GetStandingsByCompetitionAndSeasonQueryHandler(IMatchesDbContext context, IMapper mapper, IDistributedCache distributedCache)
    {
        _context = context;
        _mapper = mapper;
        _distributedCache = distributedCache;
    }

    public async Task<Result<List<Ranking>>> Handle(GetStandingsByCompetitionAndSeasonQuery query, CancellationToken cancellationToken)
    {
        string key = $"standings-{query.CompetitionId}+{query.Season}";

        var cachedStandings = await _distributedCache.GetStringAsync(key, cancellationToken);

        List<Ranking> standings;

        if (string.IsNullOrEmpty(cachedStandings))
        {
            standings = GetStandings(query);

            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(standings), cancellationToken);

            return Result<List<Ranking>>.Success(standings);
        }

        standings = JsonSerializer.Deserialize<List<Ranking>>(cachedStandings)!;

        return Result<List<Ranking>>.Success(standings);
    }

    private List<Ranking> GetStandings(GetStandingsByCompetitionAndSeasonQuery query)
    {
        var standings = _context.Matches
                                .Where(x => x.CompetitionId == query.CompetitionId)
                                .Where(x => x.Season == query.Season)
                                .Where(x => x.Status == Status.Finished)
                                .Select(x => new MatchForTeam { TeamId = x.HomeTeamId, GoalsScored = x.HomeGoals, GoalsConceded = x.AwayGoals })
                        .Concat(
                                 _context.Matches
                                .Where(x => x.CompetitionId == query.CompetitionId)
                                .Where(x => x.Season == query.Season)
                                .Where(x => x.Status == Status.Finished)
                                .Select(x => new MatchForTeam { TeamId = x.AwayTeamId, GoalsScored = x.AwayGoals, GoalsConceded = x.HomeGoals }))
                .GroupBy(x => x.TeamId)
                .ProjectTo<Ranking>(_mapper.ConfigurationProvider)
                .ToList();

        standings = standings.OrderByDescending(x => x.Points).ThenByDescending(x => x.GoalsDiff).ToList();

        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Position = i + 1;
        }

        return standings;
    }
}

public class MatchForTeam
{
    public Guid TeamId { get; set; }
    public int? GoalsScored { get; set; }
    public int? GoalsConceded { get; set; }
}