using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Services;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantModel>> GetTenantsAsync();
        Task<IEnumerable<TenantModel>> GetTenantsAsync(string partnerId);

        Task<TenantModel> GetTenantAsync(string id);

        public Task CreateAsync(TenantModel newModel);

        public Task UpdateAsync(string id, TenantModel updatedModel);

        public Task RemoveAsync(string id);
    }
}
