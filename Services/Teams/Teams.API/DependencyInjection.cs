using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            //opt.TokenValidationParameters = new()
            //{
            //    ValidateIssuer = true,
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = true,
            //    ValidIssuer = jwtSettings.Issuer,
            //    ValidAudience = jwtSettings.Audience,
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            //};
        });

        return services;
    }
}
