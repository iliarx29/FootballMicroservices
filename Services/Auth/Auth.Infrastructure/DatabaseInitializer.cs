using Auth.Infrastructure.Settings;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;
public static class DatabaseInitializer
{
    public static void PopulateIdentityServer(IApplicationBuilder app, IConfiguration configuration)
    {
        var identityServerSettings = configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>();

        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            context.Database.Migrate();

            context.ApiScopes.ExecuteDelete();
            context.ApiResources.ExecuteDelete();
            context.Clients.ExecuteDelete();
            context.IdentityResources.ExecuteDelete();

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in identityServerSettings.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in identityServerSettings.ApiResources)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                foreach (var client in identityServerSettings.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in identityServerSettings.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

        }
    }
}
