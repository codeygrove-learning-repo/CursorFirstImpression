using AspireLearning.Contracts.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspireLearning.Repository
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderRepository(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _orderCollection = database.GetCollection<Order>(settings.OrderCollectionName);
        }

        public async Task<List<Order>> GetAllAsync() =>
            await _orderCollection.Find(_ => true).ToListAsync();

        public async Task<Order> GetByIdAsync(string id) =>
            await _orderCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order order) =>
            await _orderCollection.InsertOneAsync(order);

        public async Task UpdateAsync(string id, Order order) =>
            await _orderCollection.ReplaceOneAsync(x => x.Id == id, order);

        public async Task DeleteAsync(string id) =>
            await _orderCollection.DeleteOneAsync(x => x.Id == id);

        public async Task SetDeliveredAsync(string orderId)
        {
            var update = Builders<Order>.Update.Set(o => o.Delivered, true);
            await _orderCollection.UpdateOneAsync(o => o.Id == orderId, update);
        }
    }
} 