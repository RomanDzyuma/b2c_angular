using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public interface ICredentialsSource
    {
        Task<X509SigningCredentials> GetCredentialsAsync();
    }
}
