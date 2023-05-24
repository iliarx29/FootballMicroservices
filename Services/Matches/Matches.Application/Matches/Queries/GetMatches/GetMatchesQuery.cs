using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatches;
public record GetMatchesQuery() : IRequest<Result<IEnumerable<Match>>>;

