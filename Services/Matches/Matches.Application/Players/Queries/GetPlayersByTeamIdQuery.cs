using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Players.Queries;
public record GetPlayersByTeamIdQuery(Guid TeamId) : IRequest<List<Player>>;

public class GetPlayersByTeamIdQueryHandler : IRequestHandler<GetPlayersByTeamIdQuery, List<Player>>
{
    private readonly IMatchesDbContext _context;

    public GetPlayersByTeamIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Player>> Handle(GetPlayersByTeamIdQuery query, CancellationToken cancellationToken)
    {
        var players = await _context.Players.AsNoTracking().Where(x => x.TeamId == query.TeamId).ToListAsync(cancellationToken);

        return players;
    }
}
