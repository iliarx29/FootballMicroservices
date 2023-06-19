using AutoMapper;
using MassTransit.Testing;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Nest;
using Status = Matches.Domain.Entities.Enums.Status;

namespace Matches.Application.Matches.Commands.CreateMatch;
public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<MatchResponse>>
{
    private readonly IMatchesDbContext _context;
    private readonly IDistributedCache _distributedCache;
    private readonly IMapper _mapper;
    private readonly IElasticClient _elasticClient;

    public CreateMatchCommandHandler(IMatchesDbContext context, IDistributedCache distributedCache, IMapper mapper, IElasticClient elasticClient)
    {
        _context = context;
        _distributedCache = distributedCache;
        _mapper = mapper;
        _elasticClient = elasticClient;
    }

    public async Task<Result<MatchResponse>> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = _mapper.Map<Match>(command);

        _context.Matches.Attach(match);
        await _context.SaveChangesAsync(cancellationToken);

        match = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == match.Id, cancellationToken);

        if(match is null)
        {
            return Result<MatchResponse>.Error(ErrorCode.NotFound, $"Match doesn't exist'");
        }

        var matchSearchResponse = _mapper.Map<MatchSearchResponse>(match);

        var response = await _elasticClient.IndexDocumentAsync(matchSearchResponse, cancellationToken);

        if(response.IsValid)
        {
            Console.WriteLine($"matchSearchResponse: {matchSearchResponse.Id} was indexed");
        }

        if (match.Status == Status.Finished)
        {
            string key1 = $"standings-{command.CompetitionId}+{command.Season}";
            string key2 = $"matchesByTeamId-{command.HomeTeamId}";
            string key3 = $"matchesByTeamId-{command.AwayTeamId}";

            await _distributedCache.RemoveAsync(key1, cancellationToken);
            await _distributedCache.RemoveAsync(key2, cancellationToken);
            await _distributedCache.RemoveAsync(key3, cancellationToken);
        }

        var responseMatch = _mapper.Map<MatchResponse>(match);

        return Result<MatchResponse>.Success(responseMatch);
    }
}
