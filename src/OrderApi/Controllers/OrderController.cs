using AspireLearning.Contracts.Models;
using AspireLearning.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Azure.Messaging.ServiceBus;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace AspireLearning.OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _repo;
        private readonly CatalogRepository _catalogRepo;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName = "orders";

        public OrderController(IOptions<MongoDbSettings> settings)
        {
            _repo = new OrderRepository(settings.Value);
            _catalogRepo = new CatalogRepository(settings.Value);
            _serviceBusClient = new ServiceBusClient("<SERVICEBUS_CONNECTION_STRING>");
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get() => await _repo.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var catalogs = await _catalogRepo.GetRandomAsync(2);
            var order = new Order
            {
                Id = System.Guid.NewGuid().ToString(),
                OrderDate = System.DateTime.UtcNow,
                Delivered = false,
                OrderItems = new List<OrderItem>()
            };
            foreach (var cat in catalogs)
            {
                order.OrderItems.Add(new OrderItem
                {
                    CatalogItemId = cat.Id,
                    OrderedQuantity = 1,
                    SuppliedQuantity = 0
                });
            }
            await _repo.CreateAsync(order);
            // Publish to Service Bus
            var sender = _serviceBusClient.CreateSender(_queueName);
            var message = new ServiceBusMessage(JsonSerializer.Serialize(new { Type = "OrderPlaced", OrderId = order.Id }));
            await sender.SendMessageAsync(message);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("deliver/{id}")]
        public async Task<IActionResult> MarkDelivered(string id)
        {
            await _repo.SetDeliveredAsync(id);
            // Publish delivery event to Service Bus
            var sender = _serviceBusClient.CreateSender(_queueName);
            var message = new ServiceBusMessage(JsonSerializer.Serialize(new { Type = "OrderDelivered", OrderId = id }));
            await sender.SendMessageAsync(message);
            return NoContent();
        }
    }
} 