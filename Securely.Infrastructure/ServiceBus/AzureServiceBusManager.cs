using System.Text.Json;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Securely.Infrastructure.ServiceBus;

public class AzureServiceBusManager : IServiceBusManager
{
    private readonly IOptions<ServiceBusOptions> _options;
    private readonly ILogger<AzureServiceBusManager> _logger;

    public AzureServiceBusManager(IOptions<ServiceBusOptions> options, ILogger<AzureServiceBusManager> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task SendMessageAsync<T>(T message) where T : IEventMessage
    {
        try
        {
            _logger.LogInformation($"Sending message to service bus queue {_options.Value.QueueName}.");
            await using var client = new ServiceBusClient(_options.Value.ConnectionString);
            var sender = client.CreateSender(_options.Value.QueueName);

            var messageBody = JsonSerializer.Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(messageBody);

            await sender.SendMessageAsync(serviceBusMessage);
            _logger.LogInformation($"Sent message to service bus queue {_options.Value.QueueName} successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while sending message to service bus queue {_options.Value.QueueName}.", ex);
            throw;
        }
    }
}
