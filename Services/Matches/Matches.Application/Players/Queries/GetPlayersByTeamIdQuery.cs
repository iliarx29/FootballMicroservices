using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Players.Queries;
public record GetPlayersByTeamIdQuery(Guid TeamId) : IRequest<Result<List<Player>>>;

public class GetPlayersByTeamIdQueryHandler : IRequestHandler<GetPlayersByTeamIdQuery, Result<List<Player>>>
{
    private readonly IMatchesDbContext _context;

    public GetPlayersByTeamIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Player>>> Handle(GetPlayersByTeamIdQuery query, CancellationToken cancellationToken)
    {
        var players = await _context.Players.AsNoTracking().Where(x => x.TeamId == query.TeamId).ToListAsync(cancellationToken);

        return Result<List<Player>>.Success(players);
    }
}
