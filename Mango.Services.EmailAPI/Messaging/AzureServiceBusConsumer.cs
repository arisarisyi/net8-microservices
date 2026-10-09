using System.Text;
using Mango.Services.EmailAPI.Models.Dto;
using Newtonsoft.Json;

namespace Mango.Services.EmailAPI.Messaging;
using Azure.Messaging.ServiceBus;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly IConfiguration _configuration;
    private readonly string serviceBusConnectionString;
    private string emailCartQueue;
    
    private ServiceBusProcessor _emailCartProcessor;
    
    public AzureServiceBusConsumer(IConfiguration configuration)
    {
        _configuration = configuration;
        
        serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
        
        emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
        
        var client = new ServiceBusClient (serviceBusConnectionString);
        _emailCartProcessor = client.CreateProcessor (emailCartQueue);
    }


    public async Task StartAsync()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
        _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
    }

    private Task ErrorHandler(ProcessErrorEventArgs arg)
    {
        Console.WriteLine(arg.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        var body = Encoding.UTF8.GetString(message.Body);
        
        CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
        try
        {
            await arg.CompleteMessageAsync(arg.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task StopAsync()
    {
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();
    }
}