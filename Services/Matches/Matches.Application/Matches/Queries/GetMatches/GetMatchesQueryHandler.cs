using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Options;
using Matches.Application.Results;
using MediatR;
using Microsoft.Extensions.Options;

namespace Matches.Application.Matches.Queries.GetMatches;
public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, Result<List<MatchResponse>>>
{
    private readonly IMatchesRepository _matchesRepository;
    private readonly IMapper _mapper;
    private readonly IElasticService _elasticService;
    private readonly ElasticSearchOptions _options;

    public GetMatchesQueryHandler(IMatchesRepository matchesRepository, IMapper mapper, IOptions<ElasticSearchOptions> options, IElasticService elasticService)
    {
        _matchesRepository = matchesRepository;
        _mapper = mapper;
        _options = options.Value;
        _elasticService = elasticService;
    }

    public async Task<Result<List<MatchResponse>>> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
    {
        var matches = await _matchesRepository.GetMatchesAsync(cancellationToken);

        if (!_elasticService.CheckIndexExists(_options.IndexName))
        {
            await CreateIndex();

            var searchMatches = _mapper.Map<List<MatchSearchResponse>>(matches);

            await _elasticService.AddDocumentsToElastic(searchMatches, _options.IndexName);
        }

        var matchesResponse = _mapper.Map<List<MatchResponse>>(matches);

        return Result<List<MatchResponse>>.Success(matchesResponse);
    }

    private async Task CreateIndex()
    {
        await _elasticService.CreateIndex(_options.IndexName, c => c
                .Map<MatchSearchResponse>(mm => mm
                    .AutoMap()
                    .Properties(p => p
                        .Completion(c => c
                            .Name(n => n.HomeTeamName)
                            .Analyzer("simple")
                            )
                        )
                    )
                );
    }
}

