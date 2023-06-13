using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatches;
public record GetMatchesQuery() : IRequest<Result<IEnumerable<MatchResponse>>>;

