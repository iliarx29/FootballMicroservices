using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
public record GetStandingsByLeagueAndSeasonQuery(Guid LeagueId, string Season) : IRequest<List<Ranking>>;
