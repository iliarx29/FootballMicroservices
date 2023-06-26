using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchesByTeamId;
public record GetMatchesByTeamIdQuery(Guid TeamId) : IRequest<Result<List<MatchResponse>>>;
