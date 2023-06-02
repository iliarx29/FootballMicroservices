﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Teams.API.Settings;

namespace Teams.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(opt =>
        {
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.Authority = "https://localhost:7191";
            opt.Audience = "Teams";
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("read_access", policy =>
            {
                policy.RequireRole("Admin", "User");
                policy.RequireClaim("scope", "teams.readaccess", "teams.fullaccess");
            });

            options.AddPolicy("write_access", policy =>
            {
                policy.RequireRole("Admin");
                policy.RequireClaim("scope", "teams.writeaccess", "teams.fullaccess");
            });
        });

        return services;
    }
}
