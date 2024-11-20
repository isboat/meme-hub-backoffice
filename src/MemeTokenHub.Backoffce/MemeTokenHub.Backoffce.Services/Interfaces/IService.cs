using System.Linq.Expressions;
using MemeTokenHub.Backoffce.Models;

namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetByFilter(Func<T, bool> filter);

        Task<T?> GetAsync(string id);

        public Task UpdateAsync(string id, T updatedModel);

        public Task RemoveAsync(string id);

        public Task CreateAsync(T newModel);
    }
}
