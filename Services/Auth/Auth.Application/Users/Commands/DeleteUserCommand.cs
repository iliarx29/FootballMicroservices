using Auth.Application.Results;
using MediatR;

namespace Auth.Application.Users.Commands;
public record DeleteUserCommand(Guid Id) : IRequest<Result>;
