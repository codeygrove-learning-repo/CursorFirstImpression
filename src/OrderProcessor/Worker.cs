using AspireLearning.Contracts.Models;
using AspireLearning.Repository;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace AspireLearning.OrderProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly EventHubProducerClient _eventHubProducerClient;
        private readonly OrderRepository _orderRepo;
        private readonly string _queueName = "orders";
        private readonly string _eventHubName = "order-delivered";

        public Worker(IOptions<MongoDbSettings> settings)
        {
            _serviceBusClient = new ServiceBusClient("<SERVICEBUS_CONNECTION_STRING>");
            _eventHubProducerClient = new EventHubProducerClient("<EVENTHUB_CONNECTION_STRING>", _eventHubName);
            _orderRepo = new OrderRepository(settings.Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var processor = _serviceBusClient.CreateProcessor(_queueName, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += async args =>
            {
                var body = args.Message.Body.ToString();
                var msg = JsonSerializer.Deserialize<OrderEventMessage>(body);
                if (msg != null)
                {
                    if (msg.Type == "OrderDelivered" && msg.OrderId != null)
                    {
                        await _orderRepo.SetDeliveredAsync(msg.OrderId);
                        // Publish to Event Hub
                        using EventDataBatch eventBatch = await _eventHubProducerClient.CreateBatchAsync();
                        eventBatch.TryAdd(new EventData(JsonSerializer.Serialize(new { OrderId = msg.OrderId, Delivered = true })));
                        await _eventHubProducerClient.SendAsync(eventBatch);
                        Console.WriteLine($"Order delivered event processed and published for OrderId: {msg.OrderId}");
                    }
                    else if (msg.Type == "OrderPlaced")
                    {
                        Console.WriteLine($"Order placed event received for OrderId: {msg.OrderId}");
                    }
                }
                await args.CompleteMessageAsync(args.Message);
            };
            processor.ProcessErrorAsync += args =>
            {
                Console.WriteLine($"Error: {args.Exception.Message}");
                return Task.CompletedTask;
            };
            await processor.StartProcessingAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            await processor.StopProcessingAsync();
        }
    }

    public class OrderEventMessage
    {
        public string? Type { get; set; }
        public string? OrderId { get; set; }
    }
}
