using Auth.Application.Users.Common;
using Auth.Domain.Entities;

namespace Auth.Application;
public static class Extensions
{
    public static UserResponse AsUserResponse(this User user)
    {
        return new UserResponse(new Guid(user.Id), user.UserName, user.Email, user.FirstName, user.LastName);
    }
}
