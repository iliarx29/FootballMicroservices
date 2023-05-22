using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchesByCompetitionId;
public record GetMatchesByCompetitionIdQuery(Guid CompetitionId) : IRequest<IEnumerable<Match>>;
