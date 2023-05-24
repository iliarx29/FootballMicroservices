using Matches.Application.Abstractions;
using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matches.Application.Players.Queries;
public record GetPlayerByIdQuery(Guid Id) : IRequest<Result<Player>>;

public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Result<Player>>
{
    private readonly IMatchesDbContext _context;

    public GetPlayerByIdQueryHandler(IMatchesDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Player>> Handle(GetPlayerByIdQuery query, CancellationToken cancellationToken)
    {
        var player = await _context.Players.AsNoTracking().FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (player is null)
            return Result<Player>.Error(ErrorCode.NotFound, $"Player with id: '{query.Id}' not found");

        return Result<Player>.Success(player);
    }
}
