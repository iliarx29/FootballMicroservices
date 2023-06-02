using Duende.IdentityServer.Models;

namespace Auth.Infrastructure.Settings;
public static class IdentityServerSettings
{
    public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope { Name = "matches.fullaccess" },
            new ApiScope { Name = "matches.readaccess" },
            new ApiScope { Name = "matches.writeaccess" },
            new ApiScope { Name = "teams.fullaccess" },
            new ApiScope { Name = "teams.readaccess" },
            new ApiScope { Name = "teams.writeaccess" },
            new ApiScope { Name = "offline_access" },
            new ApiScope { Name = "IdentityServerApi" }
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource
            {
                Name = "Matches",
                Scopes = new []
                {
                    "matches.fullaccess",
                    "matches.readaccess",
                    "matches.writeaccess"
                },
                UserClaims = new [] {"role"}
            },
            new ApiResource
            {
                Name = "Teams",
                Scopes = new []
                {
                    "teams.fullaccess",
                    "teams.readaccess",
                    "teams.writeaccess"
                },
                UserClaims = new [] {"role"}
            },
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                AllowedGrantTypes = new[] { GrantType.AuthorizationCode },
                RequireClientSecret = false,
                RedirectUris = new[] { "urn:ietf:wg:oauth:2.0:oob" },
                AllowedScopes = new []
                {   "openid",
                    "profile",
                    "offline_access",
                    "matches.fullaccess",
                    "teams.fullaccess",
                    "IdentityServerApi",
                    "matches.readaccess",
                    "matches.writeaccess",
                    "teams.readaccess",
                    "teams.writeaccess"
                },
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                AbsoluteRefreshTokenLifetime = 1800
            }
        };
}
