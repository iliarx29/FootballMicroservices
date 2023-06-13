using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchById;
public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, Result<MatchResponse>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;

    public GetMatchByIdQueryHandler(IMatchesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<MatchResponse>> Handle(GetMatchByIdQuery query, CancellationToken cancellationToken)
    {
        var match = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (match is null)
            return Result<MatchResponse>.Error(ErrorCode.NotFound, $"Match with id: '{query.Id}' not found");

        var matchResponse = _mapper.Map<MatchResponse>(match);

        return Result<MatchResponse>.Success(matchResponse);
    }
}
