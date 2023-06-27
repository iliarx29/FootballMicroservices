using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.SearchMatches;
public record SearchMatchesQuery(string SearchValue): IRequest<Result<List<MatchSearchResponse>>>;
