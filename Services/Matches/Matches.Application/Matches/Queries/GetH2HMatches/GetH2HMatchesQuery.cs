using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetH2HMatches;
public record GetH2HMatchesQuery(Guid Id) : IRequest<Result<IEnumerable<MatchResponse>>>;
