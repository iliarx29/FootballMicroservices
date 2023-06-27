using Matches.Application.Abstractions;
using Matches.Application.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Matches.Application.Matches.Commands.DeleteMatch;
public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, Result>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IRedisService _redisCache;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMatchCommandHandler(IMatchesRepository matchesRepository, IRedisService redisCache, IUnitOfWork unitOfWork)
    {
        _matchesRepository = matchesRepository;
        _redisCache = redisCache;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMatchCommand command, CancellationToken cancellationToken)
    {
        var match = await _matchesRepository.GetMatchByIdAsync(command.Id, cancellationToken);

        if (match is null)
            return Result.Error(ErrorCode.NotFound, $"Match with id: '{command.Id}' not found");

        _matchesRepository.DeleteMatch(match);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        string key1 = $"standings-{match.CompetitionId}+{match.Season}";
        string key2 = $"matchesByTeamId-{match.HomeTeamId}";
        string key3 = $"matchesByTeamId-{match.AwayTeamId}";

        await _redisCache.RemoveAsync(key1, cancellationToken);
        await _redisCache.RemoveAsync(key2, cancellationToken);
        await _redisCache.RemoveAsync(key3, cancellationToken);

        return Result.Success();
    }
}
