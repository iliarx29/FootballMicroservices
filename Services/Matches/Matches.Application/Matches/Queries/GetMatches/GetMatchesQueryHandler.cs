using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatches;
public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, Result<IEnumerable<MatchResponse>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;

    public GetMatchesQueryHandler(IMatchesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MatchResponse>>> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var matchesResponse = _mapper.Map<IEnumerable<MatchResponse>>(matches);

        return Result<IEnumerable<MatchResponse>>.Success(matchesResponse);
    }
}
