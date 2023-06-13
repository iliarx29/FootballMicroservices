using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public class GetMatchesByCompetitionIdQueryHandler : IRequestHandler<GetMatchesByCompetitionIdQuery, Result<IEnumerable<MatchResponse>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;

    public GetMatchesByCompetitionIdQueryHandler(IMatchesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MatchResponse>>> Handle(GetMatchesByCompetitionIdQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .Where(x => x.CompetitionId == query.CompetitionId)
            .ToListAsync(cancellationToken);

        var matchesResponse = _mapper.Map<IEnumerable<MatchResponse>>(matches);

        return Result<IEnumerable<MatchResponse>>.Success(matchesResponse);
    }
}
