using Auth.Domain.Entities;

namespace Auth.Application.Abstractions;
public interface IJwtTokenGenerator
{
    Task<string> GenerateToken(User user);
}
