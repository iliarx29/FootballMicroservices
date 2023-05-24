using Matches.Application.Result;
using Matches.Domain.Entities;
using MediatR;

namespace Matches.Application.Matches.Queries.GetMatchById;
public record GetMatchByIdQuery(Guid Id) : IRequest<Result<Match>>;
