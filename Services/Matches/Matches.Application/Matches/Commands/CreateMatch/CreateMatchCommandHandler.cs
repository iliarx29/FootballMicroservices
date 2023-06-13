using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Matches.Application.Matches.Commands.CreateMatch;
public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<Match>>
{
    private readonly IMatchesDbContext _context;
    private readonly IDistributedCache _distributedCache;
    private readonly IMapper _mapper;

    public CreateMatchCommandHandler(IMatchesDbContext context, IDistributedCache distributedCache, IMapper mapper)
    {
        _context = context;
        _distributedCache = distributedCache;
        _mapper = mapper;
    }

    public async Task<Result<Match>> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = _mapper.Map<Match>(command);

        _context.Matches.Attach(match);
        await _context.SaveChangesAsync(cancellationToken);

        if (match.Status == Status.Finished)
        {
            string key1 = $"standings-{command.CompetitionId}+{command.Season}";
            string key2 = $"matchesByTeamId-{command.HomeTeamId}";
            string key3 = $"matchesByTeamId-{command.AwayTeamId}";

            await _distributedCache.RemoveAsync(key1, cancellationToken);
            await _distributedCache.RemoveAsync(key2, cancellationToken);
            await _distributedCache.RemoveAsync(key3, cancellationToken);
        }

        return Result<Match>.Success(match);
    }
}
