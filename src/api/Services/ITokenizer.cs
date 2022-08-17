using api.Models;

namespace api.Services
{
    public interface ITokenizer
    {
        Task<string> GetTokenAsync(HttpRequest httpRequest, Invitation invitation);
        
        string GetLink(string token);
    }
}
