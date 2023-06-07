using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Teams.Domain.Interfaces;
using Teams.Domain.Services;

namespace Teams.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ICompetitionService, CompetitionService>();
        services.AddScoped<IEventBus, EventBus>();

        services.AddMassTransit(x =>
        {
            x.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            }));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
