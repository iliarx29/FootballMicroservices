using Auth.Application.Results;
using Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Users.Commands;
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly UserManager<User> _userManager;

    public DeleteUserCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            return Result.Error(ErrorCode.NotFound, "User not found");

        await _userManager.DeleteAsync(user);

        return Result.Success();
    }
}
