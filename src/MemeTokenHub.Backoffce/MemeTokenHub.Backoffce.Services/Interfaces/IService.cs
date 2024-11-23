namespace MemeTokenHub.Backoffce.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(string id);

        public Task CreateAsync(T newModel);

        public Task UpdateAsync(string id, T updatedModel);

        public Task RemoveAsync(string id);
    }
}
