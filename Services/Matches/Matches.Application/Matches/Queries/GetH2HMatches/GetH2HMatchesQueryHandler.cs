using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Matches.Application.Matches.Queries.GetH2HMatches;
public class GetH2HMatchesQueryHandler : IRequestHandler<GetH2HMatchesQuery, Result<IEnumerable<Match>>>
{
    private readonly IMatchesDbContext _context;

    public GetH2HMatchesQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Match>>> Handle(GetH2HMatchesQuery query, CancellationToken cancellationToken)
    {
        var currentMatch = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (currentMatch is null)
            return Result<IEnumerable<Match>>.Error(ErrorCode.NotFound, $"Match with id: '{query.Id}' not found");

        var h2hMatches = await _context.Matches
            .AsNoTracking()
            .Where(x => x.Status == Status.Finished)
            .Where(x => (x.HomeTeamId == currentMatch.HomeTeamId && x.AwayTeamId == currentMatch.AwayTeamId) ||
             (x.AwayTeamId == currentMatch.HomeTeamId && x.HomeTeamId == currentMatch.AwayTeamId))
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<Match>>.Success(h2hMatches);
    }
}
