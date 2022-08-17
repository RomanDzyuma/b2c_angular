using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Sg.Management.Models;
using Sg.Management.Services;

namespace Sg.Management
{
    public class Management
    {
        private readonly ITenantService _tenantService;
        private readonly ILogger _logger;

        public Management(
            ITenantService tenantService,
            ILoggerFactory loggerFactory)
        {
            _tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
            _logger = loggerFactory.CreateLogger<Management>();
        }

        [Function("management")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var userInfo = await JsonSerializer.DeserializeAsync<UserData>(req.Body);

            if (userInfo == null)
                return req.CreateResponse(HttpStatusCode.BadRequest);

            var tenantInfo = await _tenantService.GetTenantAsync(userInfo.ObjectId);

            var response = req.CreateResponse(HttpStatusCode.OK);
            var result = new UserDataExtension
            {
                Tenant = tenantInfo
            };

            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
