using Matches.Application.Result;
using MediatR;

namespace Matches.Application.Matches.Commands.DeleteMatch;
public record DeleteMatchCommand(Guid Id) : IRequest<Result>;

