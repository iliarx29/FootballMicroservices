using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatches;
public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, IEnumerable<Match>>
{
    private readonly IMatchesDbContext _context;

    public GetMatchesQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Match>> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches.AsNoTracking().ToListAsync(cancellationToken);
        if (matches is null)
            throw new ArgumentNullException(nameof(matches), "Matches is null");

        return matches;
    }
}
