using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using Matches.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetH2HMatches;
public class GetH2HMatchesQueryHandler : IRequestHandler<GetH2HMatchesQuery, Result<IEnumerable<MatchResponse>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;

    public GetH2HMatchesQueryHandler(IMatchesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MatchResponse>>> Handle(GetH2HMatchesQuery query, CancellationToken cancellationToken)
    {
        var currentMatch = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (currentMatch is null)
            return Result<IEnumerable<MatchResponse>>.Error(ErrorCode.NotFound, $"Match with id: '{query.Id}' not found");

        var h2hMatches = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .Where(x => x.Status == Status.Finished)
            .Where(x => (x.HomeTeamId == currentMatch.HomeTeamId && x.AwayTeamId == currentMatch.AwayTeamId)
                    || (x.AwayTeamId == currentMatch.HomeTeamId && x.HomeTeamId == currentMatch.AwayTeamId))
            .ToListAsync(cancellationToken);

        var h2hMatchesResponse = _mapper.Map<IEnumerable<MatchResponse>>(h2hMatches);

        return Result<IEnumerable<MatchResponse>>.Success(h2hMatchesResponse);
    }
}
