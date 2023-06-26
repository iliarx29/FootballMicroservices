using Elasticsearch.Net;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Matches.Application.Abstractions;
using Matches.Application.Behaviors;
using Matches.Application.Consumers;
using Matches.Application.Mappings;
using Matches.Application.Matches.Queries.GetStandingsByCompetitionAndSeason;
using Matches.Application.Models;
using Matches.Application.Options;
using Matches.Application.Services;
using Matches.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

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
            x.AddConsumer<TeamDeletedEventConsumer>();
            x.AddConsumer<TeamsImportedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("my-rabbitmq", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var connection = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connection;
        });

        services.AddScoped<IRedisService, RedisService>();

        services.Configure<ElasticSearchOptions>(configuration.GetSection(ElasticSearchOptions.ElasticSearch));

        services.AddSingleton<IElasticClient>(x =>
        {
            ElasticSearchOptions options = configuration.GetSection(ElasticSearchOptions.ElasticSearch).Get<ElasticSearchOptions>()!;

            var pool = new SingleNodeConnectionPool(new Uri(options.Connection));
            var settings = new ConnectionSettings(pool)
                .DefaultIndex("default-index")
                .DefaultMappingFor<MatchSearchResponse>(i => i.IndexName(options.IndexName))
                .EnableApiVersioningHeader();

            return new ElasticClient(settings);
        });

        services.AddScoped<IElasticService, ElasticService>();

        return services;
    }
}
