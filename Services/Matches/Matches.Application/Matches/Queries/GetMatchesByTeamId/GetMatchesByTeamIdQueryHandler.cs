using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Matches.Application.Matches.Queries.GetMatchesByTeamId;
public class GetMatchesByTeamIdQueryHandler : IRequestHandler<GetMatchesByTeamIdQuery, Result<IEnumerable<MatchResponse>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;

    public GetMatchesByTeamIdQueryHandler(IMatchesDbContext context, IDistributedCache distributedCache, IMapper mapper)
    {
        _context = context;
        _distributedCache = distributedCache;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MatchResponse>>> Handle(GetMatchesByTeamIdQuery request, CancellationToken cancellationToken)
    {
        string key = $"matchesByTeamId-{request.TeamId}";

        var cachedMatches = await _distributedCache.GetStringAsync(key, cancellationToken);

        IEnumerable<MatchResponse> responseMatches;

        if (string.IsNullOrEmpty(cachedMatches))
        {
            var matches = await _context.Matches
               .Include(x => x.HomeTeam)
               .Include(x => x.AwayTeam)
               .AsNoTracking()
               .Where(x => x.HomeTeamId == request.TeamId || x.AwayTeamId == request.TeamId)
               .ToListAsync(cancellationToken);

            responseMatches = _mapper.Map<IEnumerable<MatchResponse>>(matches);

            await _distributedCache.SetStringAsync(
                key, JsonSerializer.Serialize(responseMatches),
                new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1) },
                cancellationToken);

            return Result<IEnumerable<MatchResponse>>.Success(responseMatches);
        }

        responseMatches = JsonSerializer.Deserialize<IEnumerable<MatchResponse>>(cachedMatches)!;

        return Result<IEnumerable<MatchResponse>>.Success(responseMatches);
    }
}
