using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Matches.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Matches.Application.Matches.Queries.GetH2HMatches;
public class GetH2HMatchesQueryHandler : IRequestHandler<GetH2HMatchesQuery, IEnumerable<Match>>
{
    private readonly IMatchesDbContext _context;

    public GetH2HMatchesQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Match>> Handle(GetH2HMatchesQuery query, CancellationToken cancellationToken)
    {
        var currentMatch = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (currentMatch is null)
            throw new NotFoundException($"Match with id: '{query.Id}' doesn't exist.");

        var h2hMatches = await _context.Matches
            .AsNoTracking()
            .Where(x => x.Status == Status.Finished && (x.HomeTeamId == currentMatch.HomeTeamId && x.AwayTeamId == currentMatch.AwayTeamId
            || x.AwayTeamId == currentMatch.HomeTeamId && x.HomeTeamId == currentMatch.AwayTeamId))
            .ToListAsync(cancellationToken);

        return h2hMatches;
    }
}
