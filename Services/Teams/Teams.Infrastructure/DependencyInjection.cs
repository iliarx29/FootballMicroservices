using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teams.Infrastructure.Repositories.Implementations;
using Teams.Infrastructure.Repositories.Interfaces;

namespace Teams.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInsfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TeamsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITeamsRepository, TeamsRepository>();
        services.AddScoped<IUnitOfWork, TeamsDbContext>();

        return services;
    }
}
