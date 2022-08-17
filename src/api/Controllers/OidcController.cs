using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{
    [ApiController]
    public class OidcController : ControllerBase
    {
        private readonly ICredentialsSource _credentialsSource;

        public OidcController(ICredentialsSource credentialsSource)
        {
            _credentialsSource = credentialsSource;
        }

        [HttpGet(".well-known/openid-configuration", Name = "OIDCMetadata")]
        public async Task<IActionResult> Metadata()
        {
            var credentials = await _credentialsSource.GetCredentialsAsync();

            return Content(JsonConvert.SerializeObject(new OidcModel
            {
                // Sample: The issuer name is the application root path
                Issuer = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase.Value}/",

                // Sample: Include the absolute URL to JWKs endpoint
                JwksUri = Url.Link("JWKS", null),

                // Sample: Include the supported signing algorithms
                IdTokenSigningAlgValuesSupported = new[] { credentials.Algorithm }
            }), "application/json");
        }

        [HttpGet(".well-known/keys", Name = "JWKS")]
        public async Task<IActionResult> JwksDocument()
        {
            var credentials = await _credentialsSource.GetCredentialsAsync();

            return Content(JsonConvert.SerializeObject(new JwksModel
            {
                Keys = new[] { JwksModel.JwksKeyModel.FromSigningCredentials(credentials) }
            }), "application/json");
        }
    }
}
