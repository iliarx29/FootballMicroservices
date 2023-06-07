using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Matches.Application.Behaviors;
using Matches.Application.Consumers;
using Matches.Application.Mappings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Matches.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddHttpClient();

        services.AddHangfire(x => x
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer();

        services.AddScoped<ImportDataRecurringJob>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TeamCreatedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
