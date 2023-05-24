using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Teams.Domain.Interfaces;
using Teams.Domain.Services;

namespace Teams.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<ITeamService, TeamService>()
                .AddScoped<ICompetitionService, CompetitionService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
