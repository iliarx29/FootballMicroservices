using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchesByLeagueId;
public class GetMatchesLeagueIdQueryHandler : IRequestHandler<GetMatchesByLeagueIdQuery, IEnumerable<Match>>
{
    private readonly IMatchesDbContext _context;

    public GetMatchesLeagueIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Match>> Handle(GetMatchesByLeagueIdQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches.AsNoTracking().Where(x => x.LeagueId == query.LeagueId).ToListAsync(cancellationToken);

        if (matches is null)
            throw new ArgumentNullException(nameof(matches), "Matches is null");

        return matches;
    }
}
