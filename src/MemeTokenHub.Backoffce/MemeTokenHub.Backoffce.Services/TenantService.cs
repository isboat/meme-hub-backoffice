using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using MemeTokenHub.Backoffce.Services.Interfaces;
using System;

namespace MemeTokenHub.Backoffce.Services
{
    public class TenantService : ITenantService
    {
        private ITenantDBRepository<TenantModel> _repository;

        public TenantService(ITenantDBRepository<TenantModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TenantModel>> GetTenantsAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<IEnumerable<TenantModel>> GetTenantsAsync(string partnerId)
        {
            return await _repository.GetByFilter((x) => x.PartnerId == partnerId);
        }

        public async Task<TenantModel> GetTenantAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task CreateAsync(TenantModel newModel)
        {
            if (newModel != null)
            {
                newModel.Id = GenerateId(newModel.Name);
                if (string.IsNullOrEmpty(newModel.Id))
                {
                    throw new ArgumentNullException(nameof(newModel.Id));
                }

                await _repository.CreateAsync(newModel);
                _repository.CreateDB(newModel.Id);
            }
        }

        private string? GenerateId(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var id = name.Replace(" ", "_").ToLowerInvariant();
            id += "_tenant";
            return id;
        }

        public async Task UpdateAsync(string id, TenantModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }
    }
}