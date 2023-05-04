using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Matches.Queries.GetMatchById;
public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, Match>
{
    private readonly IMatchesDbContext _context;

    public GetMatchByIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Match> Handle(GetMatchByIdQuery query, CancellationToken cancellationToken)
    {
        var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (match is null)
            throw new ArgumentNullException(nameof(match), $"Match with given id: '{query.Id}' is null");

        return match;
    }
}
