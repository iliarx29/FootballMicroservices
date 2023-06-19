using AutoMapper;
using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Options;
using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nest;

namespace Matches.Application.Matches.Queries.GetMatches;
internal class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, Result<IEnumerable<MatchResponse>>>
{
    private readonly IMatchesDbContext _context;
    private readonly IMapper _mapper;
    private readonly IElasticClient _elasticClient;
    private readonly ElasticSearchOptions _options;

    public GetMatchesQueryHandler(IMatchesDbContext context, IMapper mapper, IElasticClient client, IOptions<ElasticSearchOptions> options)
    {
        _context = context;
        _mapper = mapper;
        _elasticClient = client;
        _options = options.Value;
    }

    public async Task<Result<IEnumerable<MatchResponse>>> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
    {
        var matches = await _context.Matches
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (!_elasticClient.Indices.Exists(_options.IndexName).Exists)
        {
            await CreateIndex();

            var searchMatches = _mapper.Map<List<MatchSearchResponse>>(matches);

            await AddDocumentsToElastic(searchMatches);
        }

        var matchesResponse = _mapper.Map<IEnumerable<MatchResponse>>(matches);

        return Result<IEnumerable<MatchResponse>>.Success(matchesResponse);
    }

    private async Task AddDocumentsToElastic(List<MatchSearchResponse> searchMatches)
    {
        var response = await _elasticClient.IndexManyAsync(searchMatches, _options.IndexName);

        if (response.IsValid)
        {
            Console.WriteLine($"Matches was indexed");
        }
    }

    private async Task CreateIndex()
    {
        var response = await _elasticClient.Indices.CreateAsync(_options.IndexName, c => c
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
