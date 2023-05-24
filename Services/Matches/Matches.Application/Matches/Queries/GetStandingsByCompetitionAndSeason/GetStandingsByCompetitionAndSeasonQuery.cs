using Matches.Application.Results;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
public record GetStandingsByCompetitionAndSeasonQuery(Guid CompetitionId, string Season) : IRequest<Result<List<Ranking>>>;
