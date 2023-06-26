using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchById;
public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, Result<MatchResponse>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IMapper _mapper;

    public GetMatchByIdQueryHandler(IMapper mapper, IMatchesRepository matchesRepository)
    {
        _mapper = mapper;
        _matchesRepository = matchesRepository;
    }

    public async Task<Result<MatchResponse>> Handle(GetMatchByIdQuery query, CancellationToken cancellationToken)
    {
        var match = await _matchesRepository.GetMatchByIdAsync(query.Id, cancellationToken);

        if (match is null)
            return Result<MatchResponse>.Error(ErrorCode.NotFound, $"Match with id: '{query.Id}' not found");

        var matchResponse = _mapper.Map<MatchResponse>(match);

        return Result<MatchResponse>.Success(matchResponse);
    }
}
