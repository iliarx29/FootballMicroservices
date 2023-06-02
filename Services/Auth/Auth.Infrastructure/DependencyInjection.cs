using Auth.Application.Abstractions;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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

        services.AddAuthentication()
            .AddOpenIdConnect("Google", "Google", options =>
            {
                IConfigurationSection googleAuthNSection = configuration.GetSection("Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
                options.Authority = "https://accounts.google.com";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.CallbackPath = "/signin-google";
            });

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
