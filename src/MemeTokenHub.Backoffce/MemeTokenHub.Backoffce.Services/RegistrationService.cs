using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using MemeTokenHub.Backoffce.Services.Interfaces;

namespace MemeTokenHub.Backoffce.Services
{
    public class RegistrationService : IRegistrationService
    {
        private IRepository<RegisterModel> _repository;

        public RegistrationService(IRepository<RegisterModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RegisterModel>> GetTenantsAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<RegisterModel> GetTenantAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }
    }
}