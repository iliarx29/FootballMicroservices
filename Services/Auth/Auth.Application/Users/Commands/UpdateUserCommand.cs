using Auth.Application.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Users.Commands;
public record UpdateUserCommand(
    Guid Id,
    [Required] [EmailAddress] string Email,
    string Username,
    string FirstName,
    string LastName) : IRequest<Result>;
