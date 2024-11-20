using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;

namespace MemeTokenHub.Backoffce.Services
{
    public class BaseService<T>
    {

        protected readonly IRepository<T> _repository;

        public BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public virtual async Task<T> GetAsync(string id)
        {
            return await _repository.GetAsync(id);
        }

        public virtual async Task CreateAsync(T newModel)
        {
            if (newModel != null)
            {
                await _repository.CreateAsync(newModel);
            }
        }


        public virtual async Task UpdateAsync(string id, T updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        public virtual async Task RemoveAsync(string id)
        {
            await _repository.RemoveAsync(id);
        }

        protected string? GenerateId()
        {
            var id = Guid.NewGuid().ToString("N");
            return id;
        }
    }
}