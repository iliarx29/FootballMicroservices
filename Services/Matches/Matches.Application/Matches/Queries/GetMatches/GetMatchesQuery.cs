using Matches.Application.Models;
using Matches.Application.Results;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatches;
public record GetMatchesQuery() : IRequest<Result<List<MatchResponse>>>;

