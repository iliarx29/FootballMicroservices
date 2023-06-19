using Matches.Application.Models;
using MediatR;

namespace Matches.Application.Matches.Queries.SearchMatches;
public record SearchMatchesQuery(string SearchValue): IRequest<List<MatchSearchResponse>>;
