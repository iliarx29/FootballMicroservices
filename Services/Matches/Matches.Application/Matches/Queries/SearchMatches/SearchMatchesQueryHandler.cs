using Matches.Application.Models;
using MediatR;
using Nest;

namespace Matches.Application.Matches.Queries.SearchMatches;
internal class SearchMatchesQueryHandler : IRequestHandler<SearchMatchesQuery, List<MatchSearchResponse>>
{
    private readonly IElasticClient _elasticClient;

    public SearchMatchesQueryHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<List<MatchSearchResponse>> Handle(SearchMatchesQuery request, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.SearchAsync<MatchSearchResponse>(s => s
            .Query(q => q
                .MultiMatch(a => a
                    .Fields(f => f.Field(m => m.HomeTeamName).Field(m => m.AwayTeamName))
                    .Query(request.SearchValue)
                    .Operator(Operator.Or))
                )
            .Sort(sort => sort.Descending(p => p.MatchDate))
            .Size(10), cancellationToken);

        if (response is not null)
        {
            return response.Documents.ToList();
        }

        return new();
    }
}
