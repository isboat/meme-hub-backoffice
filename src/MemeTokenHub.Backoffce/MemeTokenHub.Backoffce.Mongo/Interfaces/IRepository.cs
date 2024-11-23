
namespace MemeTokenHub.Backoffce.Mongo.Interfaces
{
    public interface IRepository<T>
    {
        public Task<List<T>> GetAsync();

        public Task<IEnumerable<T>> GetByFilter(Func<T, bool> filter);

        public Task<T?> GetAsync(string id);

        public Task CreateAsync(T newModel);

        public Task UpdateAsync(string id, T updatedModel);

        public Task RemoveAsync(string id);
    }
}
