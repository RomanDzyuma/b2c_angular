namespace Sg.Management.Services
{
    public interface ITenantService
    {
        Task<string> GetTenantAsync(string userObjectId);
    }
}