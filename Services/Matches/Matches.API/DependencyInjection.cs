using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Matches.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.Authority = "https://localhost:7191";
            opt.Audience = "Matches";
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("read_access", policy =>
            {
                policy.RequireRole("Admin", "User");
                policy.RequireClaim("scope", "matches.readaccess", "matches.fullaccess");
            });

            options.AddPolicy("write_access", policy =>
            {
                policy.RequireRole("Admin");
                policy.RequireClaim("scope", "matches.writeaccess", "matches.fullaccess");
            });
        });

        return services;
    }
}
