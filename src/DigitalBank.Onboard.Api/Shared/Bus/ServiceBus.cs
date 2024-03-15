//Create Service Bus Abstraction class
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading.Tasks;

public class ServiceBus
{
    private readonly IQueueClient _queueClient;

    public ServiceBus(string serviceBusConnectionString, string queueName)
    {
        _queueClient = new QueueClient(serviceBusConnectionString, queueName);
    }

    public async Task SendMessageAsync(string message)
    {
        var messageBody = new Message(Encoding.UTF8.GetBytes(message));
        await _queueClient.SendAsync(messageBody);
    }

    public async Task CloseQueueAsync()
    {
        await _queueClient.CloseAsync();
    }
}
