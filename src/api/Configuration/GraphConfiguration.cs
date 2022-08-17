using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;

namespace api.Configuration
{
    public static class GraphConfiguration
    {
        public static IServiceCollection ConfigureGraphClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var graphConfig = configuration.GetSection("GraphApi");
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            TokenCredential credentials = new ClientSecretCredential(
                graphConfig["TenantId"],
                graphConfig["ClientId"],
                graphConfig["ClientSecret"]);

            services.AddScoped(_ => new GraphServiceClient(credentials, scopes));

            return services;
        }
    }
}
