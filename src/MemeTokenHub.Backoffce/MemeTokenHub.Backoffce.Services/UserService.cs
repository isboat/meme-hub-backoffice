using Microsoft.Extensions.Options;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using MemeTokenHub.Backoffce.Services.Interfaces;

namespace MemeTokenHub.Backoffce.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private IEncryptionService _encryptionService;

        public UserService(IUserRepository repository, IEncryptionService encryptionService)
        {
            _repository = repository;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string tenantId)
        {
            var models = await _repository.GetUsersAsync(tenantId);
            models.ForEach(x => x.Password = null);

            return models;
        }

        public async Task<UserModel> GetAsync(string id)
        {
            var model = await _repository.GetAsync(id);
            if(model != null) model.Password = null;

            return model!;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            var model = await _repository.GetByEmailAsync(email);
            if (model != null) model.Password = null;

            return model!;
        }

        public async Task CreateAsync(UserModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.Password = _encryptionService.Encrypt("Temporary!")?.Hashed;

            await _repository.CreateAsync(newModel);
        }

        public async Task UpdateAsync(string id, UserModel updatedModel)
        {
            if (updatedModel == null) return;

            EnsureIdNotNull(updatedModel);
            updatedModel.Password = _encryptionService.Encrypt(updatedModel.Password!)?.Hashed;
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }


        private static void EnsureIdNotNull(UserModel newModel)
        {
            if (string.IsNullOrEmpty(newModel?.Id))
            {
                throw new ArgumentNullException(nameof(newModel.Id));
            }

            if (string.IsNullOrEmpty(newModel?.TenantId))
            {
                throw new ArgumentNullException(nameof(newModel.TenantId));
            }
        }
    }
}