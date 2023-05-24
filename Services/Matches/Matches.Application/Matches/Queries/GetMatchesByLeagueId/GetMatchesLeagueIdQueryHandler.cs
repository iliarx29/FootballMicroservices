using Matches.Application.Abstractions;
using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchesByLeagueId;
public class GetMatchesLeagueIdQueryHandler : IRequestHandler<GetMatchesByLeagueIdQuery, Result<IEnumerable<Match>>>
{
    private readonly IMatchesDbContext _context;

    public GetMatchesLeagueIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Match>>> Handle(GetMatchesByLeagueIdQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches.AsNoTracking().Where(x => x.LeagueId == query.LeagueId).ToListAsync(cancellationToken);

        return Result<IEnumerable<Match>>.Success(matches);
    }
}
