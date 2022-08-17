using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Configuration;
using api.Models;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public class Tokenizer: ITokenizer
    {
        private readonly IOptions<TokenizerOptions> _options;
        private readonly ICredentialsSource _credentialsSource;

        public Tokenizer(
            IOptions<TokenizerOptions> options,
            ICredentialsSource credentialsSource)
        {
            _options = options;
            _credentialsSource = credentialsSource;
        }

        public async Task<string> GetTokenAsync(
            HttpRequest request, 
            Invitation invitation)
        {
            string issuer = $"{request.Scheme}://{request.Host}{request.PathBase.Value}/";

            var claims = new List<Claim>
            {
                new("name", invitation.Name ?? string.Empty, ClaimValueTypes.String, issuer),
                new("email", invitation.Email ?? string.Empty, ClaimValueTypes.String, issuer),
                new("groupId", invitation.GroupId ?? string.Empty, ClaimValueTypes.String, issuer)
            };

            var credentials = await _credentialsSource.GetCredentialsAsync();

            var token = new JwtSecurityToken(
                issuer,
                _options.Value.ClientId,
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(7),
                credentials);

            var jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }

        public string GetLink(string token)
        {
            string nonce = Guid.NewGuid().ToString("N");

            return string.Format(
                _options.Value.SignUpUrl,
                _options.Value.Instance,
                _options.Value.Domain,
                _options.Value.Policy,
                _options.Value.ClientId,
                Uri.EscapeDataString(_options.Value.RedirectUri),
                nonce) + "&id_token_hint=" + token;
        }
    }
}
