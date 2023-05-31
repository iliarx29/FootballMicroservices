using Auth.Application.Results;
using Auth.Application.Users.Common;
using Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Users.Queries;
internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<IEnumerable<UserResponse>>>
{
    private readonly UserManager<User> _userManager;

    public GetUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.ToListAsync();

        if (users is null)
            return Result<IEnumerable<UserResponse>>.Error(ErrorCode.NotFound, "Users not found");

        var usersResponse = users.Select(user => user.AsUserResponse());

        return Result<IEnumerable<UserResponse>>.Success(usersResponse);
    }
}
