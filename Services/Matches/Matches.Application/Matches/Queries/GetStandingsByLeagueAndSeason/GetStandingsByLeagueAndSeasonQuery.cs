using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetStandingsByLeagueAndSeason;
public record GetStandingsByLeagueAndSeasonQuery(Guid LeagueId, Guid SeasonId) : IRequest<Result<List<Ranking>>>;
