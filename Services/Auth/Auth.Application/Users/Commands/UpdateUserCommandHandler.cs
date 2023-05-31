using Auth.Application.Results;
using Auth.Application.Users.Common;
using Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Commands;
internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            return Result.Error(ErrorCode.NotFound, "User not found");

        user.UserName = request.Username;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;

        await _userManager.UpdateAsync(user);

        return Result.Success();

    }
}
