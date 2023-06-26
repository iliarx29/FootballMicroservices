using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using Matches.Domain.Entities;
using Matches.Domain.Entities.Enums;
using MediatR;

namespace Matches.Application.Matches.Commands.CreateMatch;
public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<Match>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisCache;
    private readonly IMapper _mapper;
    private readonly IElasticService _elasticService;

    public CreateMatchCommandHandler(IRedisService redisCache, IMapper mapper, IMatchesRepository matchesRepository, IUnitOfWork unitOfWork, IElasticService elasticService)
    {
        _redisCache = redisCache;
        _mapper = mapper;
        _matchesRepository = matchesRepository;
        _unitOfWork = unitOfWork;
        _elasticService = elasticService;
    }

    public async Task<Result<Match>> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
    {
        var match = _mapper.Map<Match>(command);

        _matchesRepository.AddMatch(match);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (match is null)
        {
            return Result<Match>.Error(ErrorCode.NotFound, $"Match doesn't exist'");
        }

        var matchSearchResponse = _mapper.Map<MatchSearchResponse>(match);

        await _elasticService.AddDocument(matchSearchResponse, cancellationToken);

        if (match.Status == Status.Finished)
        {
            string key1 = $"standings-{command.CompetitionId}+{command.Season}";
            string key2 = $"matchesByTeamId-{command.HomeTeamId}";
            string key3 = $"matchesByTeamId-{command.AwayTeamId}";

            await _redisCache.RemoveAsync(key1, cancellationToken);
            await _redisCache.RemoveAsync(key2, cancellationToken);
            await _redisCache.RemoveAsync(key3, cancellationToken);
        }

        return Result<Match>.Success(match);
    }
}
