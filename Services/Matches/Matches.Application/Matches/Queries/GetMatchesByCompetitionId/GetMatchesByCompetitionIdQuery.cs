using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public record GetMatchesByCompetitionIdQuery(Guid LeagueId) : IRequest<Result<IEnumerable<Match>>>;
