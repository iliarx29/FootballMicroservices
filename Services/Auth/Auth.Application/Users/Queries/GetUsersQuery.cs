using Auth.Application.Results;
using Auth.Application.Users.Common;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Users.Queries;
public record GetUsersQuery() : IRequest<Result<IEnumerable<UserResponse>>>;
