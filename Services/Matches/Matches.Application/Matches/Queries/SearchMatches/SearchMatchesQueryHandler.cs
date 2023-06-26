using Matches.Application.Abstractions;
using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;
using Nest;

namespace Matches.Application.Matches.Queries.SearchMatches;
public class SearchMatchesQueryHandler : IRequestHandler<SearchMatchesQuery, Result<List<MatchSearchResponse>>>
{
    private readonly IElasticService _elasticService;

    public SearchMatchesQueryHandler(IElasticService elasticClient)
    {
        _elasticService = elasticClient;
    }

    public async Task<Result<List<MatchSearchResponse>>> Handle(SearchMatchesQuery request, CancellationToken cancellationToken)
    {
        var response = await _elasticService.SearchAsync<MatchSearchResponse>(s => s
            .Query(q => q
                .MultiMatch(a => a
                    .Fields(f => f.Field(m => m.HomeTeamName).Field(m => m.AwayTeamName))
                    .Query(request.SearchValue)
                    .Operator(Operator.Or))
                )
            .Sort(sort => sort.Descending(p => p.MatchDate))
            .Size(10), cancellationToken);

        if (response is null)
        {
            return Result<List<MatchSearchResponse>>.Error(ErrorCode.NotFound, "Matches not found");
        }

        var result = response.Documents.ToList();
        return Result<List<MatchSearchResponse>>.Success(result);
    }
}
