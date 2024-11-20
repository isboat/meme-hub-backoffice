using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Mongo.Interfaces;
using System.Collections;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Linq;

namespace MemeTokenHub.Backoffce.Mongo
{
    public class PartnerRepository : IRepository<PartnerModel>
    {
        private readonly IMongoCollection<PartnerModel> _collection;
        private readonly MongoClient _client;

        public PartnerRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase("TenantAdmin");

            _collection = mongoDatabase.GetCollection<PartnerModel>("Partners");
        }

        public async Task<List<PartnerModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<PartnerModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();


        public async Task CreateAsync(PartnerModel newTenant) =>
            await _collection.InsertOneAsync(newTenant);

        public async Task UpdateAsync(string id, PartnerModel updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<IEnumerable<PartnerModel>> GetByFilterAsync(Expression<Func<PartnerModel, bool>> filter)
        {
            var first = await _collection.AsQueryable().FirstOrDefaultAsync(filter);
            return new[] { first };
        }
        public async Task<IEnumerable<PartnerModel>> GetByFilter(Func<PartnerModel, bool> filter)
        {
            var first = _collection.AsQueryable().FirstOrDefault(filter);
            if(first == null) return Enumerable.Empty<PartnerModel>();

            return new List<PartnerModel> { first };
        }
    }
}