using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public class GetMatchesByCompetitionIdQueryHandler : IRequestHandler<GetMatchesByCompetitionIdQuery, Result<IEnumerable<MatchResponse>>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IMapper _mapper;

    public GetMatchesByCompetitionIdQueryHandler(IMatchesRepository matchesRepository, IMapper mapper)
    {
        _matchesRepository = matchesRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MatchResponse>>> Handle(GetMatchesByCompetitionIdQuery query, CancellationToken cancellationToken)
    {
        var matches = await _matchesRepository.GetMatchesByCompetitionId(query.CompetitionId, cancellationToken);

        var matchesResponse = _mapper.Map<IEnumerable<MatchResponse>>(matches);

        return Result<IEnumerable<MatchResponse>>.Success(matchesResponse);
    }
}
