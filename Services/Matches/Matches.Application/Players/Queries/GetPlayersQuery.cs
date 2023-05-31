using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Players.Queries;
public record GetPlayersQuery() : IRequest<Result<List<Player>>>;

public class GetPlayersQueryHandler : IRequestHandler<GetPlayersQuery, Result<List<Player>>>
{
    private readonly IMatchesDbContext _context;

    public GetPlayersQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Player>>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
    {
        var players = await _context.Players.AsNoTracking().ToListAsync(cancellationToken);

        return Result<List<Player>>.Success(players);
    }
}