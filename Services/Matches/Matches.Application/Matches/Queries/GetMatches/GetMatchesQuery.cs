using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatches;
public record GetMatchesQuery() : IRequest<IEnumerable<Match>>;

