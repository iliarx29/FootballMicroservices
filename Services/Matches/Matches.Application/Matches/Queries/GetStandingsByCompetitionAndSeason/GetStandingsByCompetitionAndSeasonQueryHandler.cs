using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
public class GetStandingsByCompetitionAndSeasonQueryHandler : IRequestHandler<GetStandingsByCompetitionAndSeasonQuery, Result<List<Ranking>>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IRedisService _redisCache;

    public GetStandingsByCompetitionAndSeasonQueryHandler(IMatchesRepository matchesRepository, IRedisService redisCache)
    {
        _matchesRepository = matchesRepository;
        _redisCache = redisCache;
    }

    public async Task<Result<List<Ranking>>> Handle(GetStandingsByCompetitionAndSeasonQuery query, CancellationToken cancellationToken)
    {
        string key = $"standings-{query.CompetitionId}+{query.Season}";

        var cachedStandings = await _redisCache.GetStringAsync(key, cancellationToken);

        List<Ranking> standings;

        if (string.IsNullOrEmpty(cachedStandings))
        {
            standings = GetStandings(query);

            await _redisCache.SetStringAsync(
                key,
                JsonSerializer.Serialize(standings),
                new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(2) },
                cancellationToken);

            return Result<List<Ranking>>.Success(standings);
        }

        standings = JsonSerializer.Deserialize<List<Ranking>>(cachedStandings)!;

        return Result<List<Ranking>>.Success(standings);
    }

    private List<Ranking> GetStandings(GetStandingsByCompetitionAndSeasonQuery query)
    {
        var standings = _matchesRepository.GetStandingsByCompetitionAndSeason(query.CompetitionId, query.Season);

        standings = standings.OrderByDescending(x => x.Points).ThenByDescending(x => x.GoalsDiff).ToList();

        for (int i = 0; i < standings.Count; i++)
        {
            standings[i].Position = i + 1;
        }

        return standings;
    }
}