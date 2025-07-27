using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspireLearning.VendorResupplyConsumer
{
    public class Program
    {
        private const string eventHubConnectionString = "<EVENTHUB_CONNECTION_STRING>";
        private const string eventHubName = "order-delivered";
        private const string consumerGroup = "vendor";

        public static async Task Main(string[] args)
        {
            await using var consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionString, eventHubName);
            Console.WriteLine("VendorResupplyConsumer started. Listening for events...");
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(CancellationToken.None))
            {
                if (partitionEvent.Data?.EventBody != null)
                {
                    string data = partitionEvent.Data.EventBody.ToString();
                    Console.WriteLine($"[VendorResupplyConsumer] Received event: {data}");
                }
            }
        }
    }
}
