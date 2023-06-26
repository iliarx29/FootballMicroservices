using Matches.Application.Abstractions;
using Matches.Infrastructure.Mappings;
using Matches.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Matches.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInsfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MatchesDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMatchesDbContext>(sp =>
            sp.GetRequiredService<MatchesDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<MatchesDbContext>());

        services.AddScoped<IMatchesRepository, MatchesRepository>();

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
