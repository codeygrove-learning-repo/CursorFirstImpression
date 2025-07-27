using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspireLearning.InvoiceConsumer
{
    public class Program
    {
        private const string eventHubConnectionString = "<EVENTHUB_CONNECTION_STRING>";
        private const string eventHubName = "order-delivered";
        private const string consumerGroup = "invoice";

        public static async Task Main(string[] args)
        {
            await using var consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionString, eventHubName);
            Console.WriteLine("InvoiceConsumer started. Listening for events...");
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(CancellationToken.None))
            {
                if (partitionEvent.Data?.EventBody != null)
                {
                    string data = partitionEvent.Data.EventBody.ToString();
                    Console.WriteLine($"[InvoiceConsumer] Received event: {data}");
                }
            }
        }
    }
}
