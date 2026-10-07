using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus;

public class MessageBus : IMessageBus
{

    private string _connectionString = "Endpoint..";
    
    public async Task PublishMessage(object message, string topic_queue_Name)
    {
        await using var client = new ServiceBusClient(_connectionString);
        
        ServiceBusSender sender = client.CreateSender(topic_queue_Name);
        
        var jsonMessage = JsonConvert.SerializeObject(message);
        ServiceBusMessage msg = new ServiceBusMessage(Encoding
            .UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };
        
        await sender.SendMessageAsync(msg);
        await client.DisposeAsync();
    }
}