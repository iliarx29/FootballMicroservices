using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Players.Queries;
public record GetPlayerByIdQuery(Guid Id) : IRequest<Player>;

public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Player>
{
    private readonly IMatchesDbContext _context;

    public GetPlayerByIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Player> Handle(GetPlayerByIdQuery query, CancellationToken cancellationToken)
    {
        var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if(player is null)
            throw new ArgumentNullException();

        return player;
    }
}
