using Matches.Application.Matches.Common.Responses;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public record GetMatchesByCompetitionIdQuery(Guid CompetitionId) : IRequest<Result<IEnumerable<MatchResponse>>>;
