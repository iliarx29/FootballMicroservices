using Auth.Application.Abstractions;
using Auth.Domain.Entities;
using Auth.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {

        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAuthDbContext, AuthDbContext>();

        services.AddAuth(configuration);

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        var migrationAssembly = typeof(DependencyInjection).GetTypeInfo().Assembly.GetName().Name;

        services.AddDefaultIdentity<User>(opt =>
        {
            opt.Password.RequiredLength = 7;
            opt.Password.RequireDigit = true;
            opt.User.RequireUniqueEmail = true;

            opt.Lockout.AllowedForNewUsers = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            opt.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

        services.AddIdentityServer(options =>
        {
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseErrorEvents = true;
        })
        .AddConfigurationStore(options =>
            options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), sql =>
            sql.MigrationsAssembly(migrationAssembly)))
        .AddOperationalStore(options =>
            options.ConfigureDbContext = builder => builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            sql => sql.MigrationsAssembly(migrationAssembly)))
        .AddAspNetIdentity<User>()
        .AddDeveloperSigningCredential();

        services.AddLocalApiAuthentication();

        return services;
    }
}
