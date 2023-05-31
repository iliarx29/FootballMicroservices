using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Teams.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInsfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TeamsDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
