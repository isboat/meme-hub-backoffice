using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<RegisterModel>> GetTenantsAsync();

        Task<RegisterModel> GetTenantAsync(string id);

        public Task RemoveAsync(string id);
    }
}
