using Matches.Application.Abstractions;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Matches.Application.Matches.Commands.DeleteMatch;
public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, Result>
{
    private readonly IMatchesDbContext _context;
    private readonly IDistributedCache _distributedCache;

    public DeleteMatchCommandHandler(IMatchesDbContext context, IDistributedCache distributedCache)
    {
        _context = context;
        _distributedCache = distributedCache;
    }

    public async Task<Result> Handle(DeleteMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (match is null)
            return Result.Error(ErrorCode.NotFound, $"Match with id: '{command.Id}' not found");

        _context.Matches.Remove(match);

        await _context.SaveChangesAsync(cancellationToken);

        string key1 = $"standings-{match.CompetitionId}+{match.Season}";
        string key2 = $"matchesByTeamId-{match.HomeTeamId}";
        string key3 = $"matchesByTeamId-{match.AwayTeamId}";

        await _distributedCache.RemoveAsync(key1, cancellationToken);
        await _distributedCache.RemoveAsync(key2, cancellationToken);
        await _distributedCache.RemoveAsync(key3, cancellationToken);

        return Result.Success();
    }
}
