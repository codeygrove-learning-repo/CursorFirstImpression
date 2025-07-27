using AspireLearning.Contracts.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspireLearning.Repository
{
    public class CatalogRepository
    {
        private readonly IMongoCollection<CatalogItem> _catalogCollection;

        public CatalogRepository(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _catalogCollection = database.GetCollection<CatalogItem>(settings.CatalogCollectionName);
        }

        public async Task<List<CatalogItem>> GetAllAsync() =>
            await _catalogCollection.Find(_ => true).ToListAsync();

        public async Task<CatalogItem> GetByIdAsync(string id) =>
            await _catalogCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(CatalogItem item) =>
            await _catalogCollection.InsertOneAsync(item);

        public async Task UpdateAsync(string id, CatalogItem item) =>
            await _catalogCollection.ReplaceOneAsync(x => x.Id == id, item);

        public async Task DeleteAsync(string id) =>
            await _catalogCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<CatalogItem>> GetRandomAsync(int count)
        {
            var pipeline = new[]
            {
                new MongoDB.Bson.BsonDocument { { "$sample", new MongoDB.Bson.BsonDocument { { "size", count } } } }
            };
            return await _catalogCollection.Aggregate<CatalogItem>(pipeline).ToListAsync();
        }
    }
} 