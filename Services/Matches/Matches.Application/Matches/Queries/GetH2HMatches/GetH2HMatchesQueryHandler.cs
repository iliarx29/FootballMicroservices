using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetH2HMatches;
public class GetH2HMatchesQueryHandler : IRequestHandler<GetH2HMatchesQuery, Result<List<MatchResponse>>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IMapper _mapper;

    public GetH2HMatchesQueryHandler(IMatchesRepository matchesRepository, IMapper mapper)
    {
        _matchesRepository = matchesRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<MatchResponse>>> Handle(GetH2HMatchesQuery query, CancellationToken cancellationToken)
    {
        var currentMatch = await _matchesRepository.GetMatchByIdAsync(query.Id, cancellationToken);

        if (currentMatch is null)
            return Result<List<MatchResponse>>.Error(ErrorCode.NotFound, $"Match with id: '{query.Id}' not found");

        var h2hMatches = await _matchesRepository.GetH2HMatchesAsync(currentMatch.HomeTeamId, currentMatch.AwayTeamId, cancellationToken);

        var h2hMatchesResponse = _mapper.Map<List<MatchResponse>>(h2hMatches);

        return Result<List<MatchResponse>>.Success(h2hMatchesResponse);
    }
}
