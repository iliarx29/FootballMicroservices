using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public class GetMatchesByCompetitionIdQueryHandler : IRequestHandler<GetMatchesByCompetitionIdQuery, IEnumerable<Match>>
{
    private readonly IMatchesDbContext _context;

    public GetMatchesByCompetitionIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Match>> Handle(GetMatchesByCompetitionIdQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches
            .AsNoTracking()
            .Where(x => x.CompetitionId == query.CompetitionId)
            .ToListAsync(cancellationToken);

        if (matches is null)
            throw new ArgumentNullException(nameof(matches), "Matches is null");

        return matches;
    }
}
