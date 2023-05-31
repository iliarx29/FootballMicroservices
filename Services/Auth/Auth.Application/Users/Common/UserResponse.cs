namespace Auth.Application.Users.Common;
public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    string FirsName,
    string LastName);
