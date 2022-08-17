using System.Security.Cryptography.X509Certificates;
using api.Configuration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class CredentialsSource : ICredentialsSource
    {
        private readonly IOptions<SecretOptions> _options;
        private X509SigningCredentials? _signingCredentials;

        public CredentialsSource(IOptions<SecretOptions> options)
        {
            _options = options;
        }
        
        public async Task<X509SigningCredentials> GetCredentialsAsync()
        {
            if (_signingCredentials != null)
                return _signingCredentials;
            _signingCredentials = await InitCredentials();
            return _signingCredentials;

        }

        private async Task<X509SigningCredentials> InitCredentials()
        {
            if (_options.Value.Uri == null)
            {
                throw new NullReferenceException("KeyVault Uri cannot be null");
            }

            var creds = new ClientSecretCredential(
                _options.Value.TenantId,
                _options.Value.ClientId,
                _options.Value.SecretValue);
            var client = new SecretClient(new Uri(_options.Value.Uri), creds);

            var response = await client.GetSecretAsync(_options.Value.CertificateName);
            var keyVaultSecret = response.Value;
            var privateKeyBytes = Convert.FromBase64String(keyVaultSecret.Value);
            var cert1 = new X509Certificate(privateKeyBytes);
            var certificate = new X509Certificate2(cert1);
            var credentials = new X509SigningCredentials(certificate);
            return credentials;
        }
    }
}
