using Auth.Application.Results;
using Auth.Application.Users.Common;
using MediatR;

namespace Auth.Application.Users.Queries;
public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserResponse>>;
