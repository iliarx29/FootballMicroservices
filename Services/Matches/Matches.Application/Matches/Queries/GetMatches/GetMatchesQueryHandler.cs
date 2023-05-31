using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatches;
public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, Result<IEnumerable<Match>>>
{
    private readonly IMatchesDbContext _context;

    public GetMatchesQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Match>>> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches.AsNoTracking().ToListAsync(cancellationToken);

        return Result<IEnumerable<Match>>.Success(matches);
    }
}
