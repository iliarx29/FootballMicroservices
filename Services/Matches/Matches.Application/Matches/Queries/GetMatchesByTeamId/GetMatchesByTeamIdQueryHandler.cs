using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Matches.Application.Matches.Queries.GetMatchesByTeamId;
public class GetMatchesByTeamIdQueryHandler : IRequestHandler<GetMatchesByTeamIdQuery, Result<List<MatchResponse>>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisCache;

    public GetMatchesByTeamIdQueryHandler(IMatchesRepository matchesRepository, IRedisService redisCache, IMapper mapper)
    {
        _matchesRepository = matchesRepository;
        _redisCache = redisCache;
        _mapper = mapper;
    }

    public async Task<Result<List<MatchResponse>>> Handle(GetMatchesByTeamIdQuery request, CancellationToken cancellationToken)
    {
        string key = $"matchesByTeamId-{request.TeamId}";

        var cachedMatches = await _redisCache.GetStringAsync(key, cancellationToken);

        List<MatchResponse> responseMatches;

        if (string.IsNullOrEmpty(cachedMatches))
        {
            var matches = await _matchesRepository.GetMatchesByTeamId(request.TeamId, cancellationToken);

            responseMatches = _mapper.Map<List<MatchResponse>>(matches);

            await _redisCache.SetStringAsync(
                key, JsonSerializer.Serialize(responseMatches),
                new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1) },
                cancellationToken);

            return Result<List<MatchResponse>>.Success(responseMatches);
        }

        responseMatches = JsonSerializer.Deserialize<List<MatchResponse>>(cachedMatches)!;

        return Result<List<MatchResponse>>.Success(responseMatches);
    }
}
