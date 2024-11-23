using Meme.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MemeTokenHub.Backoffce.Mongo
{
    public class BaseRepository<T> where T : IModelItem, new()
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly MongoClient _client;

        public BaseRepository(IOptions<MongoSettings> settings, string database, string collection)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase(database);

            _collection = mongoDatabase.GetCollection<T>(collection);
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual Task<IEnumerable<T>> GetByFilter(Func<T, bool> filter)
        {
            return Task.FromResult(_collection.AsQueryable().Where(filter));
        }

        public virtual async Task<T?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public virtual async Task CreateAsync(T newModel) =>
            await _collection.InsertOneAsync(newModel);

        public virtual async Task UpdateAsync(string id, T updatedModel) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedModel);

        public virtual async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}