using Matches.Application.Models;
using Matches.Application.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;

namespace Matches.Application.Matches.Queries.AutocompleteSearch;
internal class AutocompleteSearchQueryHandler : IRequestHandler<AutocompleteSearchQuery, List<string>>
{
    private readonly IElasticClient _elasticClient;
    private readonly ElasticSearchOptions _options;

    public AutocompleteSearchQueryHandler(IElasticClient elasticClient, IOptions<ElasticSearchOptions> options)
    {
        _elasticClient = elasticClient;
        _options = options.Value;
    }

    public async Task<List<string>> Handle(AutocompleteSearchQuery request, CancellationToken cancellationToken)
    {
        var response4 = await _elasticClient.SearchAsync<MatchSearchResponse>(s => s
            .Index(_options.IndexName)
            .Suggest(su => su
                .Completion("team-completion", cs => cs
                    .Field(f => f.HomeTeamName)
                    .Prefix(request.Query)
                    .SkipDuplicates()
                    .Size(10))), cancellationToken);

        var s = response4.Suggest["team-completion"].SelectMany(x => x.Options).Select(x => x.Text).ToList();

        return s;
    }
}
