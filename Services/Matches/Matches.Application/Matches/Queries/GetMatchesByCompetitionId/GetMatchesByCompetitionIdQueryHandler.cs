using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public class GetMatchesByCompetitionIdQueryHandler : IRequestHandler<GetMatchesByCompetitionIdQuery, Result<IEnumerable<Match>>>
{
    private readonly IMatchesDbContext _context;

    public GetMatchesByCompetitionIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Match>>> Handle(GetMatchesByCompetitionIdQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches.AsNoTracking().Where(x => x.CompetitionId == query.LeagueId).ToListAsync(cancellationToken);

        return Result<IEnumerable<Match>>.Success(matches);
    }
}
