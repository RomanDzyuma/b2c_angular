using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sg.Management.Configuration;
using Sg.Management.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
    })
    .ConfigureServices(services =>
    {
        services.AddOptions<TenantSettings>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("AzureAd").Bind(settings);
        });

        services.AddScoped<ITenantService, TenantService>();
    })
    .Build();

host.Run();
