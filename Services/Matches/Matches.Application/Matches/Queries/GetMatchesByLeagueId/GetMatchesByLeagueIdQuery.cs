using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchesByLeagueId;
public record GetMatchesByLeagueIdQuery(Guid LeagueId): IRequest<Result<IEnumerable<Match>>>;
