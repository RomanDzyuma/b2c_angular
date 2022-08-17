using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Sg.Management.Configuration;

namespace Sg.Management.Services
{
    public class TenantService: ITenantService
    {
        private readonly IOptions<TenantSettings> _options;

        public TenantService(IOptions<TenantSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<string> GetTenantAsync(string userObjectId)
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            TokenCredential credentials = new ClientSecretCredential(
                _options.Value.TenantId,
                _options.Value.ClientId,
                _options.Value.ClientSecret);

            var graphClient = new GraphServiceClient(credentials, scopes);

            var page = await graphClient.Users[userObjectId].MemberOf.Request().GetAsync();

            var groups = page.OfType<Group>().Select(g => g.DisplayName).Where(name => !string.IsNullOrEmpty(name));

            return string.Join(',', groups);
        }
    }
}
