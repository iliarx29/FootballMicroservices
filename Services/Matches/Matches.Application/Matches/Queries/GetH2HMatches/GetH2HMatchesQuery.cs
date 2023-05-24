using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetH2HMatches;
public record GetH2HMatchesQuery(Guid Id) : IRequest<Result<IEnumerable<Match>>>;
