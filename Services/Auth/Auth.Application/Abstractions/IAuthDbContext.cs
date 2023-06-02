namespace Auth.Application.Abstractions;
public interface IAuthDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
