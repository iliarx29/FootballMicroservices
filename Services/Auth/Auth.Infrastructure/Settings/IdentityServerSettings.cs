using Duende.IdentityServer.Models;

namespace Auth.Infrastructure.Settings;
public class IdentityServerSettings
{
    public IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

    public IEnumerable<ApiScope> ApiScopes { get; init; }
    public IEnumerable<ApiResource> ApiResources { get; init; }

    public IEnumerable<Client> Clients { get; init; }
}
