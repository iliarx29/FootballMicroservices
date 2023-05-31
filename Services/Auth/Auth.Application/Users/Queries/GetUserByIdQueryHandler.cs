using Auth.Application.Results;
using Auth.Application.Users.Common;
using Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Queries;
internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private readonly UserManager<User> _userManager;

    public GetUserByIdQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            return Result<UserResponse>.Error(ErrorCode.NotFound, "User not found");

        return Result<UserResponse>.Success(user.AsUserResponse());
    }
}
